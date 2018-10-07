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

  loadReport() {
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

  bindData(response: any) {

    this.chart = new Chart('canvas', {
      type: 'line',
      data: {
        labels: ['January', 'February', 'March', 'April'],
        datasets: [
          {
            data: [10, 25, 80],
            label: 'Something3',
            borderColor: '#3cba9f',
            fill: false
          },
          {
            data: [50, 88, 11],
            label: 'Something66',
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

}
