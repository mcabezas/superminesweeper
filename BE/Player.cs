using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Player
    {
        private long id;

        public long Id
        {
            get { return id; }
            set { id = value; }
        }

        private string nickName;

        public string NickName
        {
            get { return nickName; }
            set { nickName = value; }
        }


    }
}
