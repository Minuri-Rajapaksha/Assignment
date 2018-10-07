import { Component, OnInit } from '@angular/core';
import { Period } from '../../../entities/period';
import { ApiService } from '../../../services/api-service';
import API from '../../../services/api-config.json';
import { Account } from '../../../entities/account';
import { Chart } from 'chart.js';

@Component({
  selector: 'app-report-view',
  templateUrl: './report-view.component.html',
  styleUrls: ['./report-view.component.scss']
})
export class ReportViewComponent implements OnInit {

  periodList: Period[];
  accountList: Account[];
  chart = [];
  startPeriodId: number;
  endPeriodId: number;
  accountId: 0;

  constructor(private _apiService: ApiService) { }

  ngOnInit() {
    this.dailyForecast([45.45, 98, 4], [85, 101, 25]);
  }

  dailyForecast(temp_max, temp_min) {

    // tslint:disable-next-line:no-shadowed-variable
    const alldates = [11111, 222222, 33333];

    const weatherDates = [];
    // tslint:disable-next-line:no-shadowed-variable
    alldates.forEach((res) => {
        const jsdate = new Date(res * 1000);
        weatherDates.push(jsdate.toLocaleTimeString('en', { year: 'numeric', month: 'short', day: 'numeric' }));
    });

    this.chart = new Chart('canvas', {
      type: 'line',
      data: {
        labels: ['ssss', 'yyyy', 'gggg'],
        datasets: [
          {
            data: temp_min,
            borderColor: '#3cba9f',
            fill: false
          },
          {
            data: temp_max,
            borderColor: '#ffcc00',
            fill: false
          },
        ]
      },
      options: {
        legend: {
          display: true
        },
        scales: {
          xAxes: [{
            display: true
          }],
          yAxes: [{
            display: true
          }],
        }
      }
    });

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

  loadReport() {

    this.dailyForecast([25.45, 25, 25], [66, 66, 66]);

    const startPeriodDate = this.periodList.find(i => i.periodId === parseInt(<any>this.startPeriodId, 10)).periodDate;
    const endPeriodDate = this.periodList.find(i => i.periodId === parseInt(<any>this.endPeriodId, 10)).periodDate;
    if (startPeriodDate > endPeriodDate) {
      alert('Start period should smaller than end period');
      return;
    }
    this._apiService.get(API.accountPeriodBalance.getAccountBalanceForPeriod +
      `${this.accountId}/${this.startPeriodId}/${this.endPeriodId}`)
      .subscribe(res => {
        console.log(res);
        this.bindData(res);
      },
        err => {
          console.error(`Error occured retrieving account balance report ${err}`);
        });
  }

  bindData(res: Array<any>) {
    console.log(res);
    console.log(JSON.stringify(res));
  }

}
