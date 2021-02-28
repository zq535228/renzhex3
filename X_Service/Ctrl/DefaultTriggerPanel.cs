using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using X_Quartz;

namespace X_Service.Ctrl {
    public partial class DefaultTriggerPanel : UserControl {

        public string GetCronExp() {
            string re = "";
            if (radioButton01.Checked) {
                re = GetTriggerEveryDay();
            } else if (radioButton02.Checked) {
                re = GetTriggerEveryWeek();
            } else if (radioButton03.Checked) {
                re = GetTriggerEveryMonth();
            } else {
                re = "";
            }
            return re;
        }

        public DateTime GetStartDateTime() {
            //开始日期
            int iStartYear = dateTimeStartDate.Value.Year;
            int iStartMonth = dateTimeStartDate.Value.Month;
            int iStartDay = dateTimeStartDate.Value.Day;
            int iStartHour = Convert.ToInt32(startHour.Value);
            int iStartMinute = Convert.ToInt32(startMinute.Value);
            int iStartSecond = Convert.ToInt32(startSecond.Value);
            return new DateTime(iStartYear, iStartMonth, iStartDay, iStartHour, iStartMinute, iStartSecond);
        }

        public DateTime? GetEndDateTime() {
            //结束日期
            if (cbEndDatetime.Checked) {
                int iEndYear = dateTimeEndDate.Value.Year;
                int iEndMonth = dateTimeEndDate.Value.Month;
                int iEndDay = dateTimeEndDate.Value.Day;
                int iEndHour = Convert.ToInt32(endHour.Value);
                int iEndMinute = Convert.ToInt32(endMinute.Value);
                int iEndSecond = Convert.ToInt32(endSecond.Value);
                return new DateTime(iEndYear, iEndMonth, iEndDay, iEndHour, iEndMinute, iEndSecond);
            } else
                return null;
        }

        #region 初始化用内容
        public string txtTriggerName;
        public string txtTriggerGroup;

        public DateTime dtBenginTime;
        public DateTime dtEndtime;

        public DefaultTriggerPanel() {
            InitializeComponent();

            cbWeekNth.SelectedIndex = 0;
            cbWeekNDay.SelectedIndex = 0;
        }

        private void DefaultTriggerPanel_Load(object sender, EventArgs e) {
            closeRadio();
            closeCbWeek();
            groupBox01.Enabled = true;
            DateTime dt = DateTime.Now;

            dayStartHour.Value = startHour.Value = dt.Hour;
            dayStartMinute.Value = startMinute.Value = dt.Minute;
            dayStartSecond.Value = startSecond.Value = dt.Second;

        }

        #endregion

        #region 这里面没有用到两个属性，确认后可以删掉。
        public string JobName {
            get;
            set;
        }

        public string JobGroupName {
            get;
            set;
        }

        #endregion

        #region 从控件里获取设置的内容。貌似下面的里面有用到，不过感觉挺多余的。也许是为了少些一些代码吧。

        //触发小时
        private string GetTrigHour() {
            return dayStartHour.Value.ToString();
        }
        //触发分钟
        private string GetTrigMinute() {
            return dayStartMinute.Value.ToString();
        }
        //触发秒
        private string GetTrigSecond() {
            return dayStartSecond.Value.ToString();
        }

        #endregion

        #region 按什么条件添加间隔
        /// <summary>
        /// 按每天
        /// </summary>
        /// <returns></returns>
        private string GetTriggerEveryDay() {
            //TODO:检验规则是否合法
            string cronExpression = "";
            double dblStartNDay = Convert.ToDouble(nDay.Value);

            if (cbEndDatetime.Checked) {
                if (rbtEveryDay.Checked) {
                    cronExpression = string.Format("{0} {1} {2} ? * *", GetTrigSecond(), GetTrigMinute(), GetTrigHour());
                }
                if (rbtEveryWeekDay.Checked) {
                    cronExpression = string.Format("{0} {1} {2} ? * {3}", GetTrigSecond(), GetTrigMinute(), GetTrigHour(), "MON-FRI");
                }
                if (rbtEveryWeekend.Checked) {
                    cronExpression = string.Format("{0} {1} {2} ? * {3}", GetTrigSecond(), GetTrigMinute(), GetTrigHour(), "SAT-SUN");
                }
                if (rbtEveryNDay.Checked) {
                    //string cronExpression = string.Format("{0} {1} {2} {3} * ?", GetTrigHour(), GetTrigMinute(), GetTrigSecond(), strStartNDay);
                    //trigger = new CronTrigger(txtTriggerName, txtTriggerGroup, JobName, JobGroupName, GetStartDateTime(), GetEndDateTime(), cronExpression);
                    //SimpleTrigger
                }
                return cronExpression;
            } else {
                if (rbtEveryDay.Checked) {
                    cronExpression = string.Format("{0} {1} {2} ? * *", GetTrigSecond(), GetTrigMinute(), GetTrigHour());
                }
                if (rbtEveryWeekDay.Checked) {
                    cronExpression = string.Format("{0} {1} {2} ? * {3}", GetTrigSecond(), GetTrigMinute(), GetTrigHour(), "MON-FRI");
                }
                if (rbtEveryWeekend.Checked) {
                    cronExpression = string.Format("{0} {1} {2} ? * {3}", GetTrigSecond(), GetTrigMinute(), GetTrigHour(), "SAT-SUN");
                }
                if (rbtEveryNDay.Checked) {
                }
                return cronExpression;
            }
        }
        /// <summary>
        /// 按每周
        /// </summary>
        /// <returns></returns>
        private string GetTriggerEveryWeek() {
            string week = "";
            if (cbWeekSun.Checked)
                week += "1,";
            if (cbWeekMon.Checked)
                week += "2,";
            if (cbWeekTue.Checked)
                week += "3,";
            if (cbWeekWen.Checked)
                week += "4,";
            if (cbWeekThu.Checked)
                week += "5,";
            if (cbWeekFri.Checked)
                week += "6,";
            if (cbWeekSat.Checked)
                week += "7,";
            week = week.TrimEnd(',');

            string cronExpression = string.Format("{0} {1} {2} ? * {3}", GetTrigSecond(), GetTrigMinute(), GetTrigHour(), week);
            //Trigger trigger = new CronTrigger(txtTriggerName , txtTriggerGroup , JobName , JobGroupName , GetStartDateTime() , GetEndDateTime() , cronExpression);
            return cronExpression;
        }
        /// <summary>
        /// 按每月
        /// </summary>
        /// <returns></returns>
        private string GetTriggerEveryMonth() {
            string month = "";
            if (cbMonthJan.Checked)
                month += "1,";
            if (cbMonthFeb.Checked)
                month += "2,";
            if (cbMonthMar.Checked)
                month += "3,";
            if (cbMonthApe.Checked)
                month += "4,";
            if (cbMonthMay.Checked)
                month += "5,";
            if (cbMonthJune.Checked)
                month += "6,";
            if (cbMonthJuly.Checked)
                month += "7,";
            if (cbMonthAug.Checked)
                month += "8,";
            if (cbMonthSep.Checked)
                month += "9,";
            if (cbMonthOte.Checked)
                month += "10,";
            if (cbMonthNov.Checked)
                month += "11,";
            if (cbMonthDec.Checked)
                month += "12";

            if (month == "")
                month = "*";

            if (rbtWeekByWeek.Checked) {
                string strNth = "";
                switch (cbWeekNth.SelectedIndex) {
                    case 0:
                        strNth = "#1";
                        break;
                    case 1:
                        strNth = "#2";
                        break;
                    case 2:
                        strNth = "#3";
                        break;
                    case 3:
                        strNth = "#4";
                        break;
                    case 4:
                        strNth = "L";
                        break;
                }

                switch (cbWeekNDay.SelectedIndex) {
                    case 0:
                        strNth = "2" + strNth;
                        break;
                    case 1:
                        strNth = "3" + strNth;
                        break;
                    case 2:
                        strNth = "4" + strNth;
                        break;
                    case 3:
                        strNth = "5" + strNth;
                        break;
                    case 4:
                        strNth = "6" + strNth;
                        break;
                    case 5:
                        strNth = "7" + strNth;
                        break;
                    case 6:
                        strNth = "1" + strNth;
                        break;
                }

                string cronExpression = string.Format("{0} {1} {2} ? {3} {4}", GetTrigSecond(), GetTrigMinute(), GetTrigHour(), month, strNth);
                //Trigger trigger = new CronTrigger(txtTriggerName , txtTriggerGroup , JobName , JobGroupName , GetStartDateTime() , GetEndDateTime() , cronExpression);
                return cronExpression;
            } else {
                string strNDay = nWeekDay.Value.ToString();
                string cronExpression = string.Format("{0} {1} {2} {3} {4} ?", GetTrigSecond(), GetTrigMinute(), GetTrigHour(), strNDay, month);
                //Trigger trigger = new CronTrigger(txtTriggerName , txtTriggerGroup , JobName , JobGroupName , GetStartDateTime() , GetEndDateTime() , cronExpression);
                return cronExpression;
            }

        }

        #endregion

        #region 条件转换的一些界面显示。
        private void cbEndDatetime_CheckedChanged(object sender, EventArgs e) {
            if (cbEndDatetime.Checked)
                pnlEndDatetime.Enabled = true;
            else
                pnlEndDatetime.Enabled = false;
        }

        private void rbtEveryNDay_CheckedChanged(object sender, EventArgs e) {
            if (rbtEveryNDay.Checked)
                nDay.Enabled = true;
            else
                nDay.Enabled = false;
        }

        private void rbtWeekByWeek_CheckedChanged(object sender, EventArgs e) {
            if (rbtWeekByWeek.Checked) {
                nWeekDay.Enabled = false;
                cbWeekNth.Enabled = true;
                cbWeekNDay.Enabled = true;
            } else {
                nWeekDay.Enabled = true;
                cbWeekNth.Enabled = false;
                cbWeekNDay.Enabled = false;
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e) {
            string swithText = ((RadioButton)sender).Text;
            closeRadio();
            switch (swithText) {
                case "每天任务":
                    groupBox01.Enabled = true;
                    break;
                case "每周任务":
                    groupBox02.Enabled = true;
                    break;
                case "每月任务":
                    groupBox03.Enabled = true;
                    nWeekDay.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        private void CbWeek_CheckedChanged(object sender, EventArgs e) {
            string swithText = ((RadioButton)sender).Text;
            closeCbWeek();
            switch (swithText) {
                case "天(&D)":
                    nWeekDay.Enabled = true;
                    break;
                case "星期(&H)":
                    cbWeekNth.Enabled = true;
                    cbWeekNDay.Enabled = true;
                    break;
                default:
                    break;
            }
        }

        #region 用到的两个统一的方法，先关闭所有。
        //关闭执行间隔RadioButton
        private void closeRadio() {
            groupBox01.Enabled = false;
            groupBox02.Enabled = false;
            groupBox03.Enabled = false;
        }

        //关闭cbWeekNDay，cbWeekNth，nWeekDay
        private void closeCbWeek() {
            cbWeekNDay.Enabled = false;
            cbWeekNth.Enabled = false;
            nWeekDay.Enabled = false;
        }

        #endregion
        #endregion


    }
}
