using System;
using Microsoft.ML;
using Microsoft.ML.Data;

class Program
{
    public class HouseData
    {
        public float Size { get; set; }
        public float Price { get; set; }
    }

    public class Prediction
    {
        [ColumnName("Score")]
        public float Price { get; set; }
    }

    static void Main(string[] args)
    {
        MLContext mlContext = new MLContext();

        // 1. Import or create training data
        HouseData[] houseData = {
               new HouseData() { Size = 1.1F, Price = 1.2F },
               new HouseData() { Size = 1.9F, Price = 2.3F },
               new HouseData() { Size = 2.8F, Price = 3.0F },
               new HouseData() { Size = 3.4F, Price = 3.7F } };
        IDataView trainingData = mlContext.Data.LoadFromEnumerable(houseData);

        // 2. Specify data preparation and model training pipeline
        //  使用扩展方法创建训练管道。
        /*
         * 在代码片段中，Concatenate 和 Sdca 均为目录中的方法。 它们各创建一个追加到管道的 IEstimator 对象。
         * 此时，仅创建对象。 不进行任何执行操作。
         */
        var pipeline = mlContext.Transforms.Concatenate("Features", new[] { "Size" })
            .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "Price", maximumNumberOfIterations: 100));

        // 3. Train model   在管道中创建对象后，即可使用数据来训练模型。
        /*调用 Fit() 使用输入训练数据来估算模型的参数。 这称为训练模型。 请记住，上述线性回归模型有两个模型参数：偏差和权重。 在 Fit() 调用后，参数的值是已知的。 大部分模型拥有的参数比这多得多。

            可以在如何训练模型中了解有关模型训练的详细信息。
            生成的模型对象实现 ITransformer 接口。 也就是说，模型将输入数据转换为预测。
         */
        var model = pipeline.Fit(trainingData);

        // 4. Make a prediction
        /*可以将输入数据批量转换为预测，也可以一次转换一个输入。 
         * 在房屋价格示例中，我们同时执行了两种操作：
         * 为了评估模型而执行批量转换，以及为了进行新预测而执行单次转换。 
         * 让我们进行单个预测。CreatePredictionEngine() 方法接受一个输入类和一个输出类。 字段名称和/或代码属性确定模型训练和预测期间使用的数据列的名称。 有关详细信息，请参阅使用经过训练的模型进行预测。
         */
        var size = new HouseData() { Size = 2.5F };
        var price = mlContext.Model.CreatePredictionEngine<HouseData, Prediction>(model).Predict(size);

        Console.WriteLine($"Predicted price for size: {size.Size * 1000} sq ft= {price.Price * 100:C}k");

        // Predicted price for size: 2500 sq ft= $261.98k
    }
    public void CBD()
    {
        MLContext mlContext = new MLContext();
        var model = mlContext.Model;
        HouseData[] testHouseData =
        {
            new HouseData() { Size = 1.1F, Price = 0.98F },
            new HouseData() { Size = 1.9F, Price = 2.1F },
            new HouseData() { Size = 2.8F, Price = 2.9F },
            new HouseData() { Size = 3.4F, Price = 3.6F }
        };

        var testHouseDataView = mlContext.Data.LoadFromEnumerable(testHouseData);
        //var testPriceDataView = model.Transform(testHouseDataView);

        //var metrics = mlContext.Regression.Evaluate(testPriceDataView, labelColumnName: "Price");

        //Console.WriteLine($"R^2: {metrics.RSquared:0.##}");
        //Console.WriteLine($"RMS error: {metrics.RootMeanSquaredError:0.##}");

        // R^2: 0.96
        // RMS error: 0.19
    }
}