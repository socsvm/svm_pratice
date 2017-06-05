
import org.apache.spark.mllib.tree.model.DecisionTreeModel
import org.apache.spark.mllib.util.MLUtils
import org.apache.spark.rdd.RDD
import org.apache.spark.mllib.regression.LabeledPoint
import org.apache.spark.mllib.linalg.{Vectors, Vector}
import org.apache.spark.mllib.tree.configuration.Algo
import org.apache.spark.mllib.tree.impurity.{Gini, Impurity}
import org.apache.log4j.Logger
import org.apache.log4j.Level
import org.apache.spark.storage.StorageLevel
import org.apache.spark.rdd.RDD
import org.apache.spark.{ SparkConf, SparkContext }
import org.apache.spark.mllib.regression.LabeledPoint
import org.apache.spark.mllib.evaluation._
import org.apache.spark.mllib.linalg.Vectors
import org.apache.spark.mllib.feature.StandardScaler
import org.apache.spark.mllib.tree.DecisionTree
import org.apache.spark.mllib.tree.model.DecisionTreeModel
import org.joda.time._
import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs._
import org.apache.hadoop.conf._
import org.apache.hadoop.io._
import org.apache.hadoop.util._
import java.util.Date
import org.mortbay.util.ajax.JSON
import scala.util.parsing.json._
import com.google.gson.JsonDeserializationContext
import org.codehaus.jackson.JsonParser
import scala.util.parsing.json.JSON
import org.json4s._
import org.json4s.JsonDSL._
import org.json4s.jackson.JsonMethods._
import java.util.Scanner
import java.io._
import scala.io.Source
import java.io.FileReader
import org.apache.spark.sql.SQLContext
import org.json4s._
import org.dmg.pmml.True
import org.apache.commons.io.IOUtils;
import util.control.Breaks._
import org.apache.spark.ml._
import org.apache.spark.ml.classification.{LogisticRegression, OneVsRest}
import org.apache.spark.ml.evaluation.MulticlassClassificationEvaluator
import org.apache.spark.sql.SparkSession
import org.apache.spark.mllib.classification.{SVMModel, SVMWithSGD}
import org.apache.spark.mllib.evaluation.BinaryClassificationMetrics
import org.apache.spark.mllib.util.MLUtils

object SVMLearning { 
    def SVMLearning( sc:SparkContext,fs:FileSystem,SVMLearningFeature:String): String = {
        
        
// Load training data in LIBSVM format.
        val data = MLUtils.loadLibSVMFile(sc, "sample_libsvm_data2.txt")
//*pseudocode

// Split data into training (80%) and test (20%).
        val splits = data.randomSplit(Array(0.8, 0.2), seed = 11L)
        val training = splits(0).cache()
        val test = splits(1)

// Run training algorithm to build the model
        val numIterations = 100
        val model = SVMWithSGD.train(training, numIterations)

// Clear the default threshold.
        model.clearThreshold()
        val modelsavetext = model.toPMML
        val splitreg = modelsavetext.split("RegressionTable")
// Split and get individual support vector       
        val splitmark = splitreg(1).split("\"")
        val splitcoffi = splitmark(7) + "," + splitmark(11)
//        println(modelsavetext)
// Save model to local in xml format
        def f = new File("modelsave.txt")
        val pw = new PrintWriter(new File("SVMLearningSupportVector.txt"))
        pw.write(modelsavetext)
        pw.close

    
// Compute raw scores on the test set.
        val scoreAndLabels = test.map { point =>
        val score = model.predict(point.features)
        (score, point.label)
    }



    return(splitcoffi)
}

}