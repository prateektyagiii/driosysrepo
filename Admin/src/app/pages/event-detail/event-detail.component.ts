import { Component, OnInit } from '@angular/core';
import { ApicallService } from 'src/app/core/services/apicall.service';
import { EventDetailModel } from './event-detail.model';
import { Observable } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  selector: 'app-event-detail',
  templateUrl: './event-detail.component.html',
  styleUrls: ['./event-detail.component.scss']
})
export class EventDetailComponent implements OnInit {
data: any = {};
title: any;
pageNumber: boolean[] = [];
PageIndex: any = 1;
pageSize:number=5;
Name :any ='';
totalEventDetailCount: any;
id:any;
timezone:any;


  constructor(private apicallService : ApicallService,private _Activatedroute:ActivatedRoute,
    private _router:Router) { 
     this._fetchData();
     this.timezone = (new Date()).toTimeString().match(new RegExp("[A-Z](?!.*[\(])","g")).join('');
     this.timezone.toString();
  }

  ngOnInit(): void {
    this._Activatedroute.paramMap.subscribe(params => { 
      console.log(params);
       this.id = params.get('id'); 
      
   });
    this._fetchData();
  }
  _fetchData(){
    
    this.apicallService.GetEventDetail(this.id).subscribe((response : any)=>{
      console.log(response);
      this.data = response.Result;
      this.totalEventDetailCount = response.TotalRecord;
    })
  }
  
}
