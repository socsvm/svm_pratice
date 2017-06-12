
import java.net._
import java.io._
import scala.io._
import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs._
import org.apache.hadoop.conf._
import org.apache.hadoop.io._
import org.apache.hadoop.util._
import org.apache.spark.{ SparkConf, SparkContext }


object Spark_TCPIP_Read {
    def Spark_TCPIP_Read(server:ServerSocket): String = {
//Wait for GUI connect
        val serversocket = server.accept
        println("connect")
        var SVMFeatureDataSet = new BufferedReader(new InputStreamReader(serversocket.getInputStream)).readLine
//Read SVM learning feature
        val out = new PrintStream(serversocket.getOutputStream())
        out.flush()
        serversocket.close()
        return(SVMFeatureDataSet)
    }
}
