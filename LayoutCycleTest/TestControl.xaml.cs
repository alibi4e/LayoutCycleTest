using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.ObjectModel;


namespace LayoutCycleTest
{

    public static class TestRnd
    {
        public static Random Rnd { get; set; } = new Random();
    }

    public class TestGroup : ObservableCollection<TestModel>
    {
        public TestGroup()
        {
            var date = DateTime.Now.AddDays(TestRnd.Rnd.Next(-10, 10));

            GroupDate = date.ToString("yyyy-MM-dd");
        }

        public string GroupDate { get; }
    }

    public class TestModel
    {
        private static readonly string[] _str = new[]
        {
            "The Quick Brown Fox Jumps Over The Lazy Dog. The Quick Brown Fox Jumps Over The Lazy Dog. The Quick Brown Fox Jumps Over The Lazy Dog. The Quick Brown Fox Jumps Over The Lazy Dog.",
            "Ab cde Fghi Jklm Nopq Rst Uvw Xyz. Ab cde Fghi Jklm Nopq Rst Uvw Xyz. Ab cde Fghi Jklm Nopq Rst Uvw Xyz. Ab cde Fghi Jklm Nopq Rst Uvw Xyz. Ab cde Fghi Jklm Nopq Rst Uvw Xyz. Ab cde Fghi Jklm Nopq Rst Uvw Xyz.",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum"
        };

        public string UiLabel
        {
            get
            {
                var istr = TestRnd.Rnd.Next(2);
                var str = _str[istr];
                var startIndex = TestRnd.Rnd.Next(0, 11);
                var maxIndex = TestRnd.Rnd.Next(20, str.Length - 1);
                return str[startIndex..maxIndex];
            }
        }
    }

    public sealed partial class TestControl : UserControl
    {
        private readonly ObservableCollection<TestGroup> _testList = new();
        private ICollectionView _groupedCollection = null;
        private CollectionViewSource _viewSource = null;
        public TestControl()
        {
            this.InitializeComponent();

            Loaded += TestControl_Loaded;
        }

        private void TestControl_Loaded(object sender, RoutedEventArgs e)
        {
            ShowNextSeedResults();
        }


        private int _seed = 5;
        private void NextSeed(object sender, RoutedEventArgs e)
        {
            ShowNextSeedResults();
        }

        private void ShowNextSeedResults()
        {
            _seed++;
            TestRnd.Rnd = new Random(_seed);

            listView.Width = TestRnd.Rnd.Next(550, 700);
            listView.Height = TestRnd.Rnd.Next(320, 420);

            _testList.Clear();
            for (int g = 0; g < 5; g++)
            {
                var group = new TestGroup();
                for (int i = 0; i < 10; i++)
                    group.Add(new TestModel());
                _testList.Add(group);
            }

            _viewSource = new CollectionViewSource { IsSourceGrouped = true, Source = _testList };

            _groupedCollection = _viewSource.View;
            listView.ItemsSource = _groupedCollection;

            tbSeed.Text = _seed.ToString();
        }

    }
}