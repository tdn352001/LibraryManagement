﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrabyManagement.ViewModel
{
    public partial class MockFeeDataViewModel : Component
    {
        public MockFeeDataViewModel()
        {
            InitializeComponent();
        }

        public MockFeeDataViewModel(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}
