using System;
using System.Collections.Generic;
using System.Text;

namespace Banking.Bus.Options
{

    public sealed class BusOption
    {
        public string Address { get; set; } = default!;
        public int Port { get; set; }
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
