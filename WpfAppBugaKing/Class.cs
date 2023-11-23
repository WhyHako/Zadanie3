using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfAppBugaKing
{
    public static class CurrentUserManager
    {
        private static User currentUser;

        public static User CurrentUser
        {
            get { return currentUser; }
            set { currentUser = value; }
        }
    }
}
