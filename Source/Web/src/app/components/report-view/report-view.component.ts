import { Component, OnInit } from '@angular/core';
import { Period } from '../../../entities/period';
import { ApiService } from '../../../services/api-service';
import API from '../../../services/api-config.json';
import { Account } from '../../../entities/account';

@Component({
  selector: 'app-report-view',
  templateUrl: './report-view.component.html',
  styleUrls: ['./report-view.component.scss']
})
export class ReportViewComponent implements OnInit {

  periodList: Period[];
  accountList: Account[];
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
    const startPeriodDate = this.periodList.find(i => i.periodId === parseInt(<any>this.startPeriodId)).periodDate;
    const endPeriodDate = this.periodList.find(i => i.periodId === parseInt(<any>this.endPeriodId)).periodDate;
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
