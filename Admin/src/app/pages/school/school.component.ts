import { Component,  Input,  OnInit,  QueryList, ViewChildren,ElementRef, NgZone } from '@angular/core';

import { DecimalPipe} from '@angular/common';
import { Observable } from 'rxjs';
import {  Table,SearchResult } from './school.model';
import {ApicallService} from '../../core/services/apicall.service';
import { SchoolService } from './school.service';
import { AdvancedSortableDirective, SortEvent } from '../tables/advancedtable/advanced-sortable.directive';
import {PaginationService} from '../../core/services/pagination.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal ,NgbActiveModal} from '@ng-bootstrap/ng-bootstrap';
import { SafeValue } from '@angular/platform-browser';
import {  ViewChild,  Inject, PLATFORM_ID } from '@angular/core';

import { MapsAPILoader,MouseEvent } from '@agm/core';

import { isPlatformBrowser } from '@angular/common';




@Component({
  selector: 'app-school',
  templateUrl: './school.component.html',
  styleUrls: ['./school.component.scss'],
  providers:[SchoolService,DecimalPipe]
  

})
export class SchoolComponent implements OnInit {
 

  breadCrumbItems: Array<{}>;
  hideme: boolean[] = [];
  
  totalschoolCount:any=0;
  tableData:any = [];
  newSchool :any ;

  tablesV1$: Observable<Table[]>;
  tables$: Observable<Table[]>;
  total$: Observable<number>;

  submitted: boolean;
  btnType:string = "Add";
  validationform!: FormGroup;

  page: number=1;
  pageSize:number=5;
  Name :any ='';
  City:any='';
  SchoolId= 0;
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
   totalSchool: any;
   longitude :number;
  latitude :number;
  markers: any=[];
  
  @ViewChildren(AdvancedSortableDirective) headers: QueryList<AdvancedSortableDirective>;
  @ViewChild('streetviewMap', { static: true }) streetviewMap: any;
  @ViewChild('streetviewPano', { static: true }) streetviewPano: any;

     constructor(@Inject(PLATFORM_ID) private platformId: any, private mapsAPILoader: MapsAPILoader,private apicallService : ApicallService,public service : SchoolService,private modalService: NgbModal, public formBuilder: FormBuilder,public paginationService: PaginationService) {
      
     }

    ngOnInit(){
      this.breadCrumbItems = [{ label: 'OutReach' },{ label: 'School', active: true }];
      this.validationform = this.formBuilder.group({
        SchoolId:[0],
        Name: ['', [Validators.required]],
        StreetAddress1: ['', [Validators.required]],
        StreetAddress2: ['', [Validators.required]],
        City: ['', [Validators.required]],
        State: ['', [Validators.required]],
        Zipcode :['',[Validators.required]],
        Country: ['', [Validators.required]],
        Latitude :['',[Validators.required]],
        Longitude :['',[Validators.required]]

      });
      this._initPanorama();
      this._fetchData();
      // this.pageChange(this.PageIndex);
     // this.getPageSymbol(this.PageIndex);
    }


    _initPanorama() {
      debugger;
      if (isPlatformBrowser(this.platformId)) {
        this.mapsAPILoader.load().then(() => {

           const center = { lat: this.latitude, lng: this.longitude };
         // tslint:disable-next-line: no-string-literal
         const map = new window['google'].maps.Map(this.streetviewMap.nativeElement, {center, zoom: 12, scrollwheel: false });
        //  tslint:disable-next-line: no-string-literal
          const panorama = new window['google'].maps.StreetViewPanorama(
            this.streetviewPano.nativeElement, {
            position: center,
            pov: { heading: 34, pitch: 10 },
            scrollwheel: false
          });
         map.setStreetView(panorama);
        });
      }
    }


 /**
   *
   * @param position position where marker added
   */
  placeMarker(latitude:any,longitude:any) {
    debugger;
    const lat = latitude;
    const lng = longitude;
    this.markers.push({ latitude: lat, longitude: lng });
  }





  changeValue(i) {
    this.hideme[i] = !this.hideme[i];
  }

  _fetchData() {
    debugger;
    this.markers=[];
       this.apicallService.GetAllSchool(this.PageIndex,this.pageSize,this.Name,this.City).subscribe((response : any)=>{
        debugger;
       console.log(response);
       this.data = response.Result; 
       this.totalschoolCount = response.TotalRecord;
      //  this.apicallService.GetTotalSchoolCount(this.totalschoolCount);
      //  console.log(this.totalschoolCount);
       this.changeIndex();

       },(err : any) =>{console.log(err);})

   // this.getLocation();

      //  this.markers = [
      //   { latitude: 52.228973, longitude: 20.728218 }
      // ];
   }

  //  saveLocation(){
  //   debugger;
  //   const data = {
  //     latitude: this.latitude,
  //     longitude: this.longitude
  //   }
      
    
  //   this.modalService.dismissAll();

  // }

   pageChange(pageIndex: any) {
    debugger;
    this.PageIndex = pageIndex;
    this._fetchData() 
  }

  
  paginate(array, page_size, page_number) {
    debugger;
    return array.slice((page_number - 1) * page_size, page_number * page_size);
  }



    itemsPerPageChange(pageSize: number): void {
      debugger;
      this.pageSize = pageSize;
      this.PageIndex = 1;
      this._fetchData() 
    }
    

  changeIndex(){
    debugger
    this.startIndex = (this.PageIndex - 1) * this.pageSize + 1;
    this.endIndex = (this.PageIndex - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalschoolCount) {
        this.endIndex = this.totalschoolCount;
    }
  }

  Search(Name: any,City:any) {
    debugger;
    this.PageIndex =1;
    this.Name = Name;
    this.City = City;
    this._fetchData();
  }

  markerDragEnd($event: MouseEvent) {
    debugger;
    console.log($event);
    this.latitude = $event.coords.lat;
    this.longitude = $event.coords.lng;
  }

 
  getLocation() {
    debugger;
    if (navigator.geolocation) {
      navigator.geolocation.getCurrentPosition(
        (position: any) => {
          if (position) {
           
            this.latitude = position.coords.latitude;
            this.longitude = position.coords.longitude;
           
          }
        },
        (error: any) => console.log(error)
      );
    } else {
      alert('Geolocation is not supported by this browser.');
    }
  }
  

  /**
   * Modal Open
   * @param content modal content
   */
   openModal(content: any) {
    let SchoolId = this.validationform.get('SchoolId').value;
   this.getLocation();

    if(SchoolId > 0){
      debugger;
      this.btnType = "Update"
      this.placeMarker(this.latitude,this.longitude)
    }
    else{
      this.btnType = "Add"

    }
   
    this.modalService.open(content, { centered: true });
  }

  DeleteSchool(SchoolId:any){
this.SchoolId= SchoolId;
if(this.SchoolId !=0){
  this.apicallService.DeleteSchool(SchoolId).subscribe(res=>{
this._fetchData();
this.resetform();
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
openMap(mapModal:any) {
debugger;
console.log(this.markers);
  
  this.modalService.open(mapModal, { centered: true });
}   

resetform(){
    this.validationform.controls["SchoolId"].setValue(0);
    this.validationform.controls["Name"].setValue('');
    this.validationform.controls["StreetAddress1"].setValue('');
    this.validationform.controls["StreetAddress2"].setValue('');
    this.validationform.controls["City"].setValue('');
    this.validationform.controls["State"].setValue('');
    this.validationform.controls["Zipcode"].setValue('');
    this.validationform.controls["Country"].setValue('');
    this.validationform.controls["Latitude"].setValue('');
    this.validationform.controls["Longitude"].setValue('');
    this.latitude=0;
    this.longitude=0;
  
}
/**
   * save the contacts data
   */
  saveData() {
    debugger;
    this.newSchool = {
      SchoolId : this.validationform.get('SchoolId').value,
      Name: this.validationform.get('Name').value,
      StreetAddress1: this.validationform.get('StreetAddress1').value,
      StreetAddress2: this.validationform.get('StreetAddress2').value,
      City: this.validationform.get('City').value,
      State: this.validationform.get('State').value,
      Zipcode: this.validationform.get('Zipcode').value,
      Country: this.validationform.get('Country').value,
      Latitude: this.latitude,
      Longitude:  this.longitude
    }

    if(this.validationform.get('SchoolId').value != 0){
      debugger;
      this.apicallService.UpdateSchool(this.newSchool).subscribe(res => {
        if (res) {
          this._fetchData();
          this.resetform();
        }
      });

    }
    else{
        this.apicallService.AddSchool(this.newSchool).subscribe(res=>{
         if(res){
          this._fetchData();  
          this.resetform();

         }
        })
    }
     
      this.submitted = true;
      this.modalService.dismissAll();

    } 
    
    
    editClick(setval : any){
     this.validationform.controls["SchoolId"].setValue(setval.SchoolId);
     this.validationform.controls["Name"].setValue(setval.Name);
     this.validationform.controls["StreetAddress1"].setValue(setval.StreetAddress1);
     this.validationform.controls["StreetAddress2"].setValue(setval.StreetAddress2);
     this.validationform.controls["City"].setValue(setval.City);
     this.validationform.controls["State"].setValue(setval.State);
     this.validationform.controls["Zipcode"].setValue(setval.Zipcode);
     this.validationform.controls["Country"].setValue(setval.Country);
     this.validationform.controls["Latitude"].setValue(this.latitude);
     this.validationform.controls["Longitude"].setValue(this.longitude);
    }

  
 /**
   * Sort table data
   * @param param0 sort the column
   *
   */
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

  }


