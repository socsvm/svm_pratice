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
    def Spark_TCPIP_Send(ip: String , port: Int ,SVMSupportVectorOutput: String) = {
//client          
        val client_socket = new Socket(InetAddress.getByName(ip), port)
//        lazy val client_in = new BufferedSource(client_s.getInputStream()).getLines()
        val client_output = new PrintStream(client_socket.getOutputStream())
//Print process stage
        println("-------------------------------------------------------")
        println("call GUI")
        client_output.println(SVMSupportVectorOutput)
        client_output.flush()
//Close socket
        client_socket.close()
    }      
}