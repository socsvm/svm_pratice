
import java.net._
import java.io._
import scala.io._
import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs._
import org.apache.hadoop.conf._
import org.apache.hadoop.io._
import org.apache.hadoop.util._
import org.apache.spark.{ SparkConf, SparkContext }
import org.apache.log4j.Logger
import org.apache.log4j.Level

object SparkSVMLearning {
    def main(args: Array[String]): Unit = {
//Spark and HDFS setting        
        SetLogger()
        val conf = new Configuration()
        conf.set("fs.defaultFS", "hdfs://163.18.49.38:8020")
        val fs = org.apache.hadoop.fs.FileSystem.get(conf)
        val sc = new SparkContext(new SparkConf().setAppName("App").setMaster("local[4]")
                 .set("spark.executor.memory", "2g"))
//Socket server start
        val server = new ServerSocket(9999)
        var cycle = 0;
        while (true) {
            println("server ready")
            cycle = cycle +1;
            Spark_TCPIP_Read.Spark_TCPIP_Read(server)
        val serversocket = server.accept
//        println("connect")
            val SVMSupportVector = SVMLearning.SVMLearning(sc,fs)
            Spark_TCPIP_Send.Spark_TCPIP_Send(serversocket,SVMSupportVector)
        }
    }

    def SetLogger() = {
        Logger.getLogger("org").setLevel(Level.OFF)
        Logger.getLogger("com").setLevel(Level.OFF)
        System.setProperty("spark.ui.showConsoleProgress", "false")
        Logger.getRootLogger().setLevel(Level.OFF);
    }
}