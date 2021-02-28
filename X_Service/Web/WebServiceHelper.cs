using System;
#if DEBUG
using X_Service.localhost;
#else
using X_Service.org.renzhe.serv;
#endif
using X_Service.Util;

namespace X_Service.Web {
    public class WebServiceHelper {

        private ServiceShop serv;
        private int retry = 0;

        public WebServiceHelper() {
            retry = 0;

            if (serv == null) {
                serv = new ServiceShop();
                serv.Timeout = 200000;
                serv.UserAgent = "Mozilla/5.0 (Windows NT 6.1; rv:5.0) Gecko/20100101 Firefox/5.0";

            }
        }

        public ModelShopOne[] GetAllPickModules() {

            ModelShopOne[] slist = new ModelShopOne[] { };
            try {
                slist = serv.GetAllPickModules();
            } catch (Exception ex) {
                ++retry;

                if (retry < 3) {
                    this.GetAllPickModules();
                }
                EchoHelper.EchoException(ex);
            }
            return slist;
        }

        public ModelShopOne[] GetAllPutModules() {

            ModelShopOne[] slist = new ModelShopOne[] { };
            try {
                slist = serv.GetAllPutModules();
            } catch (Exception ex) {
                ++retry;

                if (retry < 3) {
                    this.GetAllPutModules();
                }
                EchoHelper.EchoException(ex);
            }
            return slist;
        }

        public string GetClassStr(string filename, mType ty) {
            string str = "";
            try {
                str = serv.GetClassStr(filename, ty);
            } catch (Exception ex) {
                ++retry;

                if (retry < 3) {
                    this.GetClassStr(filename, ty);
                }
                EchoHelper.EchoException(ex);
            }
            return str;


        }

        public bool Delete(string classMemberObj, string fileName, mType mtype) {
            bool bl = false;
            try {
                bl = serv.Delete(classMemberObj, fileName, mtype);
            } catch (Exception ex) {
                ++retry;
                if (retry < 3) {
                    this.Delete(classMemberObj, fileName, mtype);
                }
                EchoHelper.EchoException(ex);
            }
            return bl;

        }

        public bool UploadClassStr(string fileName, string fileClassStr, mType mtype) {
            bool bl = false;
            try {
                bl = serv.UploadClassStr(fileName, fileClassStr, mtype);
            } catch (Exception ex) {
                ++retry;

                if (retry < 3) {
                    this.UploadClassStr(fileName, fileClassStr, mtype);
                }
                EchoHelper.EchoException(ex);
            }
            return bl;
        }

        public bool ReName(string classMemberObj, string fileName, string fileNewName, mType mtype) {
            bool bl = false;
            try {
                bl = serv.ReName(classMemberObj, fileName, fileNewName, mtype);
            } catch (Exception ex) {
                ++retry;

                if (retry < 3) {
                    this.ReName(classMemberObj, fileName, fileNewName, mtype);
                }
                EchoHelper.EchoException(ex);
            }
            return bl;

        }

        public void doit() {
            try {
                serv.doit();
            } catch {
                ++retry;
                if (retry < 3) {
                    this.serv.doit();
                }
            }


        }

    }
}
