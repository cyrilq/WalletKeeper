using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalletKeeper
{
    class Constants
    {
        public const string START_MESSAGE = 
        @"! Welcome to the WalletKeeper - your personal spending manager.

        You can send me photos of your checks and I will keep track of your expenses available for you every time you need it.

        Press /help to get detalized instructions.";

        public const string HELP_MESSAGE =
        @"Send me a photo of a new check to have a new expense item. 
        Press /spending to get detalized information about your expensions.";

        public const string WE_HAVE_GOT_YOUR_MESSAGE = "Thank you! We have got your message and send you an answer soon.";
    }
}
