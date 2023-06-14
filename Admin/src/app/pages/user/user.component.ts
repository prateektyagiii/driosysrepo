import { Component, OnInit ,QueryList, ViewChildren} from '@angular/core';
import { DecimalPipe} from '@angular/common';
import { Observable } from 'rxjs';
import {  Table,SearchResult } from './user.model';
import {ApicallService} from '../../core/services/apicall.service'
import { UserSortableDirective, SortEvent } from './user-sortable.directive';
import {PaginationService} from '../../core/services/pagination.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SafeValue } from '@angular/platform-browser';
import { UserService } from './user.service';
import { isNull } from '@angular/compiler/src/output/output_ast';
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss'],
  providers:[UserService,DecimalPipe]
})

  export class UserComponent implements OnInit {
 
    breadCrumbItems: Array<{}>;
    hideme: boolean[] = [];
    tableData:any = [];
    newUser :any ;
  
    tablesV1$: Observable<Table[]>;
    tables$: Observable<Table[]>;
    total$: Observable<number>;
  
    page: any=1;
    pageSize:number=5;
    Firstname :any ='';
    Middlename :any ='';
    Lastname :any ='';
    UserEmail :any ='';
    Userid= 0;
    totalUser: any;
    totalUserCount: any;
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
    useractive:boolean=null;
    
  
    @ViewChildren(UserSortableDirective) headers: QueryList<UserSortableDirective>;
$user: any;
       constructor(private apicallService : ApicallService,public service : UserService,private modalService: NgbModal, public formBuilder: FormBuilder,public paginationService: PaginationService) {
      //  this.tables$ = service.tables$;
      //  this.total$ = service.total$;
       //this._fetchData();
  
       }
    //constructor(private apicallService : ApicallService) { }
  
    ngOnInit(): void {
      this.breadCrumbItems = [{ label: 'OutReach' }, { label: 'User', active: true }];
      this._fetchData();
        //this.getPageSymbol(this.PageIndex);
    }
    changeValue(i) {
      this.hideme[i] = !this.hideme[i];
    }
  
    _fetchData() {
      debugger;
       this.apicallService.GetAllUser(this.PageIndex,this.pageSize,this.Firstname,this.Middlename,this.Lastname,this.UserEmail,this.useractive).subscribe((response : any)=>{
       console.log('user response',response);
       this.data = response.Result; 
       this.totalUserCount = response.TotalRecord;
      //  this.apicallService.GetTotalSchoolCount(this.totalschoolCount);
       //console.log('Total user count',this.totalUserCount);
       this.changeIndex();
         },(err : any) =>{console.log(err);})
     }
  
     pageChange(pageIndex: any) {
  
      this.PageIndex = pageIndex;
      this._fetchData() 
    }
   
  
     itemsPerPageChange(pageSize: number): void {
      //debugger;
      this.pageSize = pageSize;
      this.PageIndex = 1;
      this._fetchData();
      // this.data = this.paginate(this.tableData, this.pageSize, this.PageIndex);
      // this.changeIndex();
  
    }

    useractivechange(useractive:any)
    {
      
      if(useractive=="Active")
      {
        this.useractive = true;
        this._fetchData();
      }
      else if(useractive=="InActive"){
        this.useractive=false;
        this._fetchData();
      }
      else{
        debugger;
        this.useractive = null;
        this._fetchData();
      }
      
      
    }
    getPageSymbol(current: number) {
      return ['1', '2', '3', '4', '5', '6', '7'][current - 1];
    }
    
    changeIndex(){
   //   debugger
      this.startIndex = (this.PageIndex - 1) * this.pageSize + 1;
      this.endIndex = (this.PageIndex - 1) * this.pageSize + this.pageSize;
      if (this.endIndex > this.totalUserCount) {
          this.endIndex = this.totalUserCount;
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

    Search(Firstname: any,Middlename:any,Lastname:any,UserEmail:any) {
      debugger;
      this.PageIndex =1;
      this.Firstname = Firstname;
      this.Middlename = Middlename;
      this.Lastname = Lastname;
      this.UserEmail = UserEmail;
      this._fetchData();
    }
    
    
  }
  