
import java.net._
import java.io._
import scala.io._
import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs._
import org.apache.hadoop.conf._
import org.apache.hadoop.io._
import org.apache.hadoop.util._
import org.apache.spark.{ SparkConf, SparkContext }


object SparkSVMLearning {
    def main(args: Array[String]): Unit = {
//Spark and HDFS setting        
        val conf = new Configuration()
        conf.set("fs.defaultFS", "hdfs://163.18.49.38:8020")
        val fs = org.apache.hadoop.fs.FileSystem.get(conf)
        val sc = new SparkContext(new SparkConf().setAppName("App").setMaster("local[4]")
                 .set("spark.executor.memory", "2g"))
//Socket server start
        val server = new ServerSocket(9999)
        while (true) {
            val SVMLearningFeature = Spark_TCPIP_Read.Spark_TCPIP_Read(server)
            val SVMSupportVector = SVMLearning.SVMLearning(sc,fs,SVMLearningFeature)
//IP and Port should be changed
            Spark_TCPIP_Send.Spark_TCPIP_Send(ip,port,SVMSupportVector)
        }
    }
}
