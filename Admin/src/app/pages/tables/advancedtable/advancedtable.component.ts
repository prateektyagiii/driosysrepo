import { Component, OnInit, ViewChildren, QueryList } from '@angular/core';
import { DecimalPipe } from '@angular/common';

import { Observable } from 'rxjs';

import { Table } from './advanced.model';

//import { tableData } from './data';
import {ApicallService} from '../../../core/services/apicall.service'

import { AdvancedService } from './advanced.service';
import { AdvancedSortableDirective, SortEvent } from './advanced-sortable.directive';

@Component({
  selector: 'app-advancedtable',
  templateUrl: './advancedtable.component.html',
  styleUrls: ['./advancedtable.component.scss'],
  providers: [AdvancedService, DecimalPipe]
})

/**
 * Advanced table component
 */
export class AdvancedtableComponent implements OnInit {
  // bread crum data
  breadCrumbItems: Array<{}>;
  hideme: boolean[] = [];

  // Table data
  tableData: Table[];

  tablesV1$: Observable<Table[]>;
  tables$: Observable<Table[]>;
  total$: Observable<number>;

  @ViewChildren(AdvancedSortableDirective) headers: QueryList<AdvancedSortableDirective>;

  constructor(public service: AdvancedService,private apicallService : ApicallService) {
    this.tables$ = service.tables$;
    this.total$ = service.total$;
  }
  ngOnInit() {

    this.breadCrumbItems = [{ label: 'Tables' }, { label: 'Advanced Table', active: true }];

    /**
     * fetch data
     */
    // this._fetchData();
  }


  changeValue(i) {
    this.hideme[i] = !this.hideme[i];
  }

  /**
   * fetches the table value
   */
//   _fetchData() {
//    var tableData;
//     this.apicallService.GetAllInterest().subscribe((response : any)=>{
//       console.log(response);
//       tableData = response.Result; 
//       this.tablesV1$ = response.Result; 
//        console.log("MyData");   
//        console.log(this.tablesV1$);    
//        },(err : any) =>{console.log(err);})
       
//    //    this.tablesV1$=tableData;
//      //  console.log("MyData1");  
//     //   console.log(this.tablesV1$);    
// ///
//     this.tableData = tableData;
//     for (let i = 0; i <= 3; i++) {
//      this.hideme.push(true);
//    }
//   }

  /**
   * Sort table data
   * @param param0 sort the column
   *
   */
  onSort({ column, direction }: SortEvent) {
    // resetting other headers
    debugger;
    this.headers.forEach(header => {
      if (header.sortable !== column) {
        header.direction = '';
      }
    });
    this.service.sortColumn = column;
    this.service.sortDirection = direction;
  }
}
