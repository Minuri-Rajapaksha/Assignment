import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../../services/api-service';
import API from '../../../services/api-config.json';
import { AccountPeriodBalance } from '../../../entities/account-period-balance';
import { Period } from '../../../entities/period';

@Component({
  selector: 'app-straightforward-view',
  templateUrl: './straightforward-view.component.html',
  styleUrls: ['./straightforward-view.component.scss']
})
export class StraightforwardViewComponent implements OnInit {

  accountPeriodBalance: AccountPeriodBalance[];
  periodList: Period[];
  periodId: number;

  constructor(private _apiService: ApiService) { }

  ngOnInit() {
    this.getAllPeriods();
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

  getAccountBalanceForPeriod(periodId: number) {
    this._apiService.get(API.accountPeriodBalance.getAccountBalanceForPeriod + `${periodId}`)
      .subscribe(res => {
        this.accountPeriodBalance = res;
      },
        err => {
          console.error(`Error occured retrieving account balance ${err}`);
        });
  }
}
