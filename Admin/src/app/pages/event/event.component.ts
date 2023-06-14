import { Component, OnInit,QueryList, ViewChildren } from '@angular/core';
import { DecimalPipe} from '@angular/common';
import { Observable } from 'rxjs';
import {  Table,SearchResult } from './event.model';
import {ApicallService} from '../../core/services/apicall.service'
//import { SchoolService } from './school.service';
import { AdvancedSortableDirective, SortEvent } from '../tables/advancedtable/advanced-sortable.directive';
import {PaginationService} from '../../core/services/pagination.service'
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { SafeValue } from '@angular/platform-browser';
import { EventService } from './event.service';
import { tableData } from '../school/data';
import { Router } from '@angular/router';
@Component({
  selector: 'app-event',
  templateUrl: './event.component.html',
  styleUrls: ['./event.component.scss'],
 providers:[EventService,DecimalPipe]
})
export class EventComponent implements OnInit {
 
  breadCrumbItems: Array<{}>;
  hideme: boolean[] = [];
  tableData:any = [];
  newEvent :any ;

  tablesV1$: Observable<Table[]>;
  tables$: Observable<Table[]>;
  total$: Observable<number>;

  page: any=1;
  pageSize:number=5;
  Name :any ='';
  City:any='';
  EventId= 0;
  totalEvent: any;
  totalEventCount: any;
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
  longitude :number;
  latitude :number;
  markers: any=[];
  zoom=8;
  timezone:any;

  @ViewChildren(AdvancedSortableDirective) headers: QueryList<AdvancedSortableDirective>;
i: any;
     constructor(private apicallService : ApicallService,public service : EventService,private modalService: NgbModal, public formBuilder: FormBuilder,public paginationService: PaginationService, private _router:Router) {
     this.tables$ = service.tables$;
     this.total$ = service.total$;
     this._fetchData();
     this.timezone = (new Date()).toTimeString().match(new RegExp("[A-Z](?!.*[\(])","g")).join('');
     this.timezone.toString();

     }
  //constructor(private apicallService : ApicallService) { }

  ngOnInit(): void {
    this.breadCrumbItems = [{ label: 'OutReach' }, { label: 'Event', active: true }];
    this._fetchData();
    this.timezone = (new Date()).toTimeString().match(new RegExp("[A-Z](?!.*[\(])","g")).join('');
    //alert(this.timezone);
    console.log(this.timezone);
      //this.getPageSymbol(this.PageIndex);
  }
 

  changeValue(i) {
    this.hideme[i] = !this.hideme[i];
  }

  _fetchData() {
       //debugger;
       this.apicallService.GetAllEvent(this.PageIndex,this.pageSize,this.City,this.Name).subscribe((response : any)=>{
       console.log('Event Response',response);
       this.data = response.Result; 
       this.totalEventCount = response.TotalRecord;
       this.changeIndex();
       this.tableData = tableData;
     
       for (let i = 0; i <= this.tableData.length; i++) {
         this.hideme.push(true);
       }

       },(err : any) =>{console.log(err);})
   }

   pageChange(pageIndex: any) {
    //debugger;
    this.PageIndex = pageIndex;
    this._fetchData() 
  }
 

   itemsPerPageChange(pageSize: number): void {
   
      this.pageSize = pageSize;
      this.PageIndex = 1;
      this._fetchData();


  }
  getPageSymbol(current: number) {
    return ['1', '2', '3', '4', '5', '6', '7'][current - 1];
  }
  changeIndex(){
    //debugger
    this.startIndex = (this.PageIndex - 1) * this.pageSize + 1;
    this.endIndex = (this.PageIndex - 1) * this.pageSize + this.pageSize;
    if (this.endIndex > this.totalEventCount) {
        this.endIndex = this.totalEventCount;
    }
  }

  Search(Name: any,City:any) {
   // debugger;
    this.PageIndex =1;
    this.Name = Name;
    this.City = City;
    this._fetchData();
  }



  eventdetail(event : any){
   // debugger;
   console.log(event);
   this._router.navigate(['/event-detail'+event]);
  }

  openMap(mapModal:any) {
  //  debugger;
    console.log(this.markers);
      
      this.modalService.open(mapModal, { centered: true });
    }   

  markerDragEnd($event: any) {
  //  debugger;
    console.log($event);
     this.latitude = $event.coords.lat;
    this.longitude = $event.coords.lng;
  }
  
getLocation() {
 //   debugger;
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
  
  placeMarker(latitude:any,longitude:any) {
   // debugger;
    const lat = latitude;
    const lng = longitude;
    this.markers.push({ latitude: lat, longitude: lng });
  } 

  compare(v1, v2) {
    return v1 < v2 ? -1 : v1 > v2 ? 1 : 0;
}

  sort(tables: Table[], column: string, direction: string): Table[] {
   // debugger;
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
   // debugger;
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
