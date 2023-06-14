import { Component, OnInit, AfterViewInit, ViewChildren, QueryList, ViewChild, Input } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { latLng, tileLayer } from 'leaflet';
import { SchoolComponent } from '../school/school.component';
import { ApicallService } from '../../core/services/apicall.service';
import {formatDate} from '@angular/common';

// import { ChartType, Stat, Chat, Transaction } from './dashboard.model';
import { ChartType, Stat, Transaction } from './dashboard.model';

import { statData, revenueChart, salesAnalytics, sparklineEarning, sparklineMonthly, chatData, transactions } from './data';
import { delay, filter } from 'rxjs/operators';

// import { ApexAxisChartSeries, ChartComponent } from "ng-apexcharts";
import {
  ApexNonAxisChartSeries,  
  ApexResponsive,
  ApexChart,
  ChartComponent,
  ApexAxisChartSeries
} from "ng-apexcharts";
import {ApexOptions} from 'apexcharts';
import * as moment from 'moment';
// import {
//   ApexNonAxisChartSeries,
//   ApexResponsive,
//   ApexChart
// } from "ng-apexcharts";


export type ChartOptions = {
  series: ApexNonAxisChartSeries;
  chart: ApexChart;
  responsive: ApexResponsive[];
  labels: any;
  colors:any;
  tooltip: ApexTooltip;
  dataLabels: ApexDataLabels;
  legend: ApexLegend;
  plotOptions: ApexPlotOptions;
};

// export type ChartOptions = {
//   series: ApexNonAxisChartSeries;
//   chart: ApexChart;
//   responsive: ApexResponsive[];
//   labels: any;
//   dataLabels:any;
//   legend:any;
//   colors:any;
// };

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})

/**
 * Dashboard Component
 */
export class DashboardComponent implements OnInit {

  // bread crumb items
   breadCrumbItems: Array<{}>;

   revenueChart: ChartType;
   salesAnalytics: ChartType;
   sparklineEarning: ChartType;
   sparklineMonthly: ChartType;
 
   // Form submit
   chatSubmit: boolean;
 total:number;
   formData: FormGroup;
   TotalNo: any = [];
 
   options = {
     layers: [
       tileLayer('http://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', { maxZoom: 18, attribution: '...' })
     ],
     zoom: 6,
     center: latLng(46.879966, -121.726909)
   };

   statData = [
    {
      icon: 'ri-user-line',
      title: 'Number of Users',
      value: 0
    }, {
      icon: 'ri-building-line',
      title: 'Number of School',
      value: 0
    }, {
      icon: 'ri-calendar-event-fill',
      title: 'Total Event',
      value: 0
    },
    {
      icon: 'ri-calendar-check-fill',
      title: 'Previous Event',
      value: 0
  },
  {
      icon: 'ri-calendar-todo-fill',
      title: 'Upcoming Event',
      value: 0
  },
  {
    icon: 'ri-calendar-todo-fill',
      title: 'Today Event',
      value: 0
  }
  ]

  term: any;
  previouseventcount:number;
  upcomingeventcount:number;
  todayeventcount:number;
  //chatData: Chat[];
  transactions: Transaction[];
  piechartdata:number[]=[];
  totalUser: number;
  totalSchool: number;
  totalEvent: number;
  totalschoolCount: number;
  totalUserCount: number;
  totalEventCount: number;
  page: number;
  pageSize: number;
  Name: any = '';
  City: any = '';
  Firstname:any ='';
  Middlename:any ='';
  Lastname:any = '';
  UserEmail:any = '';
  useractive:any = null;
  _series:number[];
  //currentDate = new Date().toISOString();
  currentDate = new Date();
  
  
  @ViewChild("chart",{ static: true }) chart: ChartComponent;
  public chartOptions: Partial<ChartOptions>;


  constructor(public formBuilder: FormBuilder, private apicallService: ApicallService) {

    this.chartOptions = {
      series: [1,1,1],
    chart:{
      type : 'donut'
    },
    labels: ["Previous Event","Upcoming Event" ,"Today Event"],
    colors: ['#5664d2', '#1cbb8c', '#eeb902'],
    dataLabels:{
      enabled:true
    },

    plotOptions: {
      pie: {
        donut: {
          size: '65%'
          
        }
      }
    },
    legend:{
show:false    }

     }
    }

  ngOnInit(): void {
    //this.currentDate.setUTCHours(0, 0, 0, 0);
    
    this.breadCrumbItems = [{ label: 'OutReach' }, { label: 'Dashboard', active: true }];
    this.formData = this.formBuilder.group({
      message: ['', [Validators.required]],
    });
    this._fetchData();


  }
 
  private async _fetchData() {
    //debugger;
    this.GetTotal();
    this.revenueChart = revenueChart;
    this.salesAnalytics = salesAnalytics;
    this.sparklineEarning = sparklineEarning;
    this.sparklineMonthly = sparklineMonthly;
    // this.chatData = chatData;
    this.transactions = transactions;

      setInterval(() => {
        debugger;
        this.chartOptions.series = [this.statData[3].value,this.statData[4].value,this.statData[5].value];
        this.total = this.statData[3].value+this.statData[4].value + this.statData[5].value

      },1000)

}


async GetTotal() {
  //debugger;
    this.apicallService.GetAllSchool(this.page, this.pageSize, this.Name, this.City).subscribe((res: any) => {
      console.log(res.TotalRecord);
      this.statData[1].value = res.TotalRecord;

    })

    this.apicallService.GetAllUser(this.page, this.pageSize, this.Firstname,this.Middlename,this.Lastname,this.UserEmail,this.useractive).subscribe((res: any) => {
      console.log(res.TotalRecord);
      this.statData[0].value = res.TotalRecord;

    })

   await this.apicallService.GetAllEvent(this.page, this.pageSize, this.City, this.Name).subscribe((res: any) => {
    //debugger 
    console.log(res);
      this.statData[2].value = res.TotalRecord;
      this.totalEventCount= res.TotalRecord;
      // this.piechartdata[0]=this.totalEventCount;
      this.DateCompare(res.Result);
     })
  }

  /**
   * Returns form
   */
  get form() {
    return this.formData.controls;
  }
/**
   * Date Compare for event
   */
  DateCompare(res: any) {
    var time=[];
    var preevent = [];
    var nextevent = [];
    var todayevent = [];
    console.log(res);
    res.forEach(ele => {
    time.push(ele.EventTime)
   
  })

    time.forEach((data :Date) => {
      debugger;
      if (data)
      { 
        debugger;

        formatDate(this.currentDate,'yyyy/MM/dd', 'en');
        formatDate(data, 'yyyy/MM/dd', 'en');
        console.log("current date ",formatDate(data, 'yyyy/MM/dd', 'en'));  
        console.log("data date ",formatDate(this.currentDate, 'yyyy/MM/dd', 'en'));        
        // data >= this.currentDate ?  nextevent.push(data) : preevent.push(data);
        formatDate(data, 'yyyy/MM/dd', 'en') > formatDate(this.currentDate,'yyyy/MM/dd', 'en') ? nextevent.push(data) :(formatDate(data, 'yyyy/MM/dd', 'en')<formatDate(this.currentDate,'yyyy/MM/dd', 'en') ? preevent.push(data) :todayevent.push(data));

      }
    });
    this.statData[3].value=preevent.length;
    this.previouseventcount = preevent.length
    // this.piechartdata[1]=this.previouseventcount;

    this.statData[4].value=nextevent.length
    this.upcomingeventcount = nextevent.length
    // this.piechartdata[2]=this.upcomingeventcount;

    this.statData[5].value = todayevent.length
    this.todayeventcount = todayevent.length
  }

  /**
   * Save the message in chat
   */
  messageSave() {
    const message = this.formData.get('message').value;
    const currentDate = new Date();
    currentDate.setHours(0, 0, 0, 0)
    console.log("current date",currentDate);
    if (this.formData.valid && message) {
      // Message Push in Chat
      // this.chatData.push({
      //   align: 'right',
      //   name: 'Ricky Clark',

      
      //   message,
      //   time: currentDate.getHours() + ':' + currentDate.getMinutes()
      // });

      // Set Form Data Reset
      this.formData = this.formBuilder.group({
        message: null
      });
    }

    this.chatSubmit = true;
  }
}
