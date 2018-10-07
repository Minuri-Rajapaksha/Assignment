import { Component, OnInit } from '@angular/core';
import { Period } from '../../../entities/period';
import { ApiService } from '../../../services/api-service';
import API from '../../../services/api-config.json';

@Component({
  selector: 'app-report-view',
  templateUrl: './report-view.component.html',
  styleUrls: ['./report-view.component.scss']
})
export class ReportViewComponent implements OnInit {

  periodList: Period[];
  accountList: Account[];

  constructor(private _apiService: ApiService) { }

  ngOnInit() {
    this.getAllPeriods();
    this.getAllAccounts();
  }

  getAllPeriods() {
    this._apiService.get(API.period.getDropdownList)
      .subscribe(res => {
        this.periodList = res;
      },
        err => {
          console.error(`Error occured retrieving periods ${err}`);
        });
  }

  getAllAccounts() {
    this._apiService.get(API.account.getDropdownList)
      .subscribe(res => {
        this.accountList = res;
      },
        err => {
          console.error(`Error occured retrieving periods ${err}`);
        });
  }

  // lineChart
  // tslint:disable-next-line:member-ordering
  public lineChartData: Array<any> = [
    { data: [65, 59, 80, 81, 56, 55, 40], label: 'R&D' },
    { data: [28, 48, 40, 19, 86, 27, 90], label: 'Canteen' },
    { data: [18, 48, 77, 9, 100, 27, 40], label: 'CEOs Car' },
    { data: [23, 43, 42, 21, 75, 40, 50], label: 'Marketing' },
    { data: [56, 87, 67, 54, 160, 87, 60], label: 'Parking fines' },
  ];
  // tslint:disable-next-line:member-ordering
  public lineChartLabels: Array<any> = ['January', 'February', 'March', 'April', 'May', 'June', 'July'];
  // tslint:disable-next-line:member-ordering
  public lineChartOptions: any = {
    responsive: true
  };
  // tslint:disable-next-line:member-ordering
  public lineChartColors: Array<any> = [
    { // grey
      backgroundColor: 'rgba(148,159,177,0.2)',
      borderColor: 'rgba(148,159,177,1)',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    },
    { // dark grey
      backgroundColor: 'rgba(77,83,96,0.2)',
      borderColor: 'rgba(77,83,96,1)',
      pointBackgroundColor: 'rgba(77,83,96,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(77,83,96,1)'
    },
    { // grey
      backgroundColor: 'rgba(148,159,177,0.2)',
      borderColor: 'rgba(148,159,177,1)',
      pointBackgroundColor: 'rgba(148,159,177,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(148,159,177,0.8)'
    },
    { // dark grey
      backgroundColor: 'rgba(77,83,96,0.2)',
      borderColor: 'rgba(77,83,96,1)',
      pointBackgroundColor: 'rgba(77,83,96,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(77,83,96,1)'
    },
    { // grey
      backgroundColor: 'rgba(145,160,167,0.2)',
      borderColor: 'rgba(145,160,167,1)',
      pointBackgroundColor: 'rgba(145,160,167,1)',
      pointBorderColor: '#fff',
      pointHoverBackgroundColor: '#fff',
      pointHoverBorderColor: 'rgba(145,160,167,0.8)'
    }
  ];
  // tslint:disable-next-line:member-ordering
  public lineChartLegend = true;
  // tslint:disable-next-line:member-ordering
  public lineChartType = 'line';

  // public randomize():void {
  //   let _lineChartData:Array<any> = new Array(this.lineChartData.length);
  //   for (let i = 0; i < this.lineChartData.length; i++) {
  //     _lineChartData[i] = {data: new Array(this.lineChartData[i].data.length), label: this.lineChartData[i].label};
  //     for (let j = 0; j < this.lineChartData[i].data.length; j++) {
  //       _lineChartData[i].data[j] = Math.floor((Math.random() * 100) + 1);
  //     }
  //   }
  //   this.lineChartData = _lineChartData;
  // }

  // events
  public chartClicked(e: any): void {
    console.log(e);
  }

  public chartHovered(e: any): void {
    console.log(e);
  }
}
