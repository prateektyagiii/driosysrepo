import { Component, OnInit,ViewChildren, QueryList } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { Observable } from 'rxjs';
import { Table } from './interest.model';
import {ApicallService} from '../../core/services/apicall.service'
import { InterestService } from './interest.service';
import {PaginationService} from '../../core/services/pagination.service';
import { InterestSortableDirective, SortEvent } from './interest-sortable.directive';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { category } from '../calendar/data';


@Component({
  selector: 'app-interest',
  templateUrl: './interest.component.html',
  styleUrls: ['./interest.component.scss'],
  providers: [InterestService, DecimalPipe]
})
export class InterestComponent implements OnInit {
  breadCrumbItems: Array<{}>;
  hideme: boolean[] = [];
  totalinterestCount: any;
  tableData:any = [];
  newInterest :any ;

  tablesV1$: Observable<Table[]>;
  tables$: Observable<Table[]>;
  total$: Observable<number>;

  submitted: boolean;
  btnType:string = "Add";
  validationform!: FormGroup;

  page: any=1;
  pageSize:number=5;
  Name :any ='';
  City:any='';
  InterestId= 0;
  

  pageField = [];
  exactPageList: any;
  paginationData!: number;
  companiesPerPage: any = 5;
  totalCompanies: any;
  totalCompaniesCount: any;
  Lastpage: any;
  pageNumber: boolean[] = [];
  PageIndex: any = 1;
  data : any;
  startIndex :number=1;
  endIndex :number =0;
  totalInterest: any;

  @ViewChildren(InterestSortableDirective) headers: QueryList<InterestSortableDirective>;
  constructor(private apicallService : ApicallService,public service : InterestService,private modalService: NgbModal, public formBuilder: FormBuilder,public paginationService: PaginationService) { 
    this._fetchData();
  }

  ngOnInit(){
    this.breadCrumbItems = [{ label: 'OutReach' }, { label: 'Interest', active: true }];
    this.validationform = this.formBuilder.group({	
      InterestId:[0],
      Name: ['', [Validators.required]],
      Category: ['', [Validators.required]],
      Icon: ['', [Validators.required]],
      Description: ['', [Validators.required]]
    });

    this._fetchData();
    //this.GetInterestList();
  }
  changeValue(i) {
    this.hideme[i] = !this.hideme[i];
  }

  
  _fetchData() {
    debugger;
    this.apicallService.GetAllInterest(this.PageIndex,this.pageSize,this.Name).subscribe((response : any)=>{
      console.log('user response',response);
      this.data = response.Result; 
      this.totalinterestCount = response.TotalRecord;
     //  this.apicallService.GetTotalSchoolCount(this.totalschoolCount);
      //console.log('Total user count',this.totalUserCount);
      this.changeIndex();
      
      // this.tableData = Table;
      for (let i = 0; i <= this.tableData.length; i++) {
        this.hideme.push(true);
      }
        },(err : any) =>{console.log(err);})
      
 } 
 pageChange(pageIndex: any) {
  this.PageIndex = pageIndex;
    this._fetchData() 
 }
 paginate(array, page_size, page_number) {
  return array.slice((page_number - 1) * page_size, page_number * page_size);
}
 itemsPerPageChange(pageSize: number): void {
  debugger;
      this.pageSize = pageSize;
      this.PageIndex = 1;

      this._fetchData();

}
 
 changeIndex(){
  debugger
  this.startIndex = (this.PageIndex - 1) * this.pageSize + 1;
  this.endIndex = (this.PageIndex - 1) * this.pageSize + this.pageSize;
  if (this.endIndex > this.totalinterestCount) {
      this.endIndex = this.totalinterestCount;
  }
  
}

compare(v1, v2) {
  return v1 < v2 ? -1 : v1 > v2 ? 1 : 0;
}

sort(tables: Table[], column: string, direction: string): Table[] {
  debugger;
  if (direction === '') {
      return tables;
  } else {
      return [...tables].sort((a, b) => {
        debugger;
          const res = this.compare(b[column], a[column]);
          return direction === 'asc' ? res : -res;
      });
  }
  
  
}

onSort(column:any,direction:any) {
  // resetting other headers
  debugger;
  this.headers.forEach(header => {
    if (header.sortable !== column) {
      header.direction = '';
    }
  });
  this.service.sortColumn = column;
  this.service.sortDirection = direction;   
  this.data = this.sort(this.data,column,direction);
}
  

  
/**
   * Modal Open
   * @param content modal content
   */
 openModal(content: any) {
  let InterestId = this.validationform.get('InterestId').value;

// if(SchoolId == null) {SchoolId = 0;}

  if(InterestId > 0){
    this.btnType = "Update"
  }
  this.modalService.open(content, { centered: true });
}
 DeleteInterest(InterestId:any){
 this.InterestId= InterestId;
 if(this.InterestId !=0){
 this.apicallService.DeleteInterest(InterestId).subscribe(res=>{
 this._fetchData();
 })
}
}
Delete(){
this.modalService.dismissAll();

}
/**
 * Returns form
 */
get form() {
return this.validationform.controls;
}
 /**

/**
 * save the contacts data
 */
saveData() {
  debugger;
  this.newInterest = {
      InterestId : this.validationform.get('InterestId').value,
      Name: this.validationform.get('Name').value,
      Category: this.validationform.get('Category').value,
      Icon: this.validationform.get('Icon').value,
      Description: this.validationform.get('Description').value,
  } 
      if(this.validationform.get('InterestId').value != 0){

        this.apicallService.UpdateInterest(this.newInterest).subscribe(res => {
          //   if (res) {
            this._fetchData();
        //console.log(res);
         // }
        });
  
      }
      else{
          this.apicallService.AddInterest(this.newInterest).subscribe(res=>{
         //  console.log(res);
       //    if(res){
            this._fetchData();
         //  }
          })
      }
 
      this.submitted = true;
      this.modalService.dismissAll();

    }

   
    editClick(setval : any){
      debugger;
      this.validationform.controls["InterestId"].setValue(setval.InterestId);
      this.validationform.controls["Name"].setValue(setval.Name);
      this.validationform.controls["Category"].setValue(setval.Category);
      this.validationform.controls["Icon"].setValue(setval.Icon);
      this.validationform.controls["Description"].setValue(setval.Description);
     
     }

     search(Name:any){
      //  debugger;
              this.PageIndex=1;
              this.Name = Name;
              this._fetchData(); 
     }

    //  



  // GetInterestList(){
  //   this.apicallService.GetAllInterest().subscribe((response : any)=>{
  //  console.log(response);
  //  this.tableData = response.Result; 
  //   console.log(this.tableData);    
  //   },(err : any) =>{console.log(err);})
    
  //  }
  
   

}
