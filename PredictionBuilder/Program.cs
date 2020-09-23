using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PredictionBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            List<SampleModel> list = new List<SampleModel>()
            {
                new SampleModel(){ Id=1, Name="A",Param1=1,Param2=2,Param3=3},
                new SampleModel(){ Id=2, Name="B",Param1=null,Param2=1,Param3=2},
                new SampleModel(){ Id=3, Name="C",Param1=1,Param2=1,Param3=2}
            };

            SampleFilter filter = new SampleFilter()
            {
                Param1 = 1,
                Param3 = new List<int?>() { 2 },
                Param2 = null
            }
            ;
            var predicate = new ExpressionBuilder<SampleModel, SampleFilter>(filter).Build();
            var result = list.AsQueryable().Where(predicate).ToList();

        }
    }
}
