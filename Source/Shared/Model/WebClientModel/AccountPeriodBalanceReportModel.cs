using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Model.WebClientModel
{
    public class AccountPeriodBalanceReportModel
    {
        public AccountPeriodBalanceReportModel()
        {
            DataSet = new List<ChartDataSet>();
            Period = new List<string>();
        }

        public List<ChartDataSet> DataSet { get; set; }
        public List<string> Period { get; set; }
    }

    public class ChartDataSet
    {
        public List<decimal> Data { get; set; }
        public string Label { get; set; }
        public string BorderColor { get; set; }
        public bool Fill { get; set; }
    }
}
