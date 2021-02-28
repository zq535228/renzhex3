using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace X_Service.Login {

    [Serializable]
    public class ModelMember {

        private string uID;

        public string UID {
            get {
                return uID;
            }
            set {
                if (value.Length < 2) {
                    value = "0" + value;
                }
                uID = value;
            }
        }
        public string netname;
        public string netpass;
        public CookieCollection cookies = null;
        public string formhash = string.Empty;
        public int sitenum = 0;
        public string hdinfo;
        public string realname;
        public string expire;
        public string other;
        public string email;
        public int userMoney = 0;
        public string group;

        public bool IS_X_WordPressBuild = false;
        public bool IS_X_PostKing = false;

        public DateTime logintime;

        public string ipaddress;
        public string strCommand;

        public string strMessage;
        public bool bLoginSuccess;

        public string sKey; //用户MD5加密的密钥。

        public ModelMember CopyTo() {
            ModelMember newobj = new ModelMember();
            newobj.uID = this.uID;
            newobj.netname = this.netname;
            newobj.netpass = this.netpass;
            newobj.cookies = this.cookies;
            newobj.formhash = this.formhash;
            newobj.sitenum = this.sitenum;
            newobj.hdinfo = this.hdinfo;
            newobj.realname = this.realname;
            newobj.expire = this.expire;
            newobj.other = this.other;
            newobj.email = this.email;
            newobj.userMoney = this.userMoney;
            newobj.group = this.group;
            newobj.IS_X_WordPressBuild = this.IS_X_WordPressBuild;
            newobj.IS_X_PostKing = this.IS_X_PostKing;

            newobj.logintime = this.logintime;
            newobj.ipaddress = this.ipaddress;
            newobj.bLoginSuccess = this.bLoginSuccess;
            newobj.strMessage = this.strMessage;

            return newobj;
        }

    }
}
