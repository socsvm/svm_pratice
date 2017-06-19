import java.net._
import java.io._
import scala.io._
import org.apache.hadoop.conf.Configuration;
import org.apache.hadoop.fs._
import org.apache.hadoop.conf._
import org.apache.hadoop.io._
import org.apache.hadoop.util._
import org.apache.spark.{ SparkConf, SparkContext }


object Spark_TCPIP_Send {
    def Spark_TCPIP_Send(serversocket:Socket ,SVMSupportVector:String) = {
//client          
        val client_output = new PrintStream(serversocket.getOutputStream())
        client_output.println(SVMSupportVector)
        client_output.flush()
//Close socket
        serversocket.close()
        println("----------------------------")
    }      
}