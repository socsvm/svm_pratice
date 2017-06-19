
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
    def Spark_TCPIP_Read(server:ServerSocket): Unit = {
//Wait for GUI connect
      var writestring = new String
        val pw = new PrintWriter(new File("SVMLearningSupportVector.txt"))
      var reading = 1
        while(reading==1){
            val serversocket = server.accept
            println("connect")
            var SVMFeatureDataSet = new BufferedReader(new InputStreamReader(serversocket.getInputStream)).readLine
            println(SVMFeatureDataSet)
//Read SVM learning feature
            val out = new PrintStream(serversocket.getOutputStream())
            out.flush()
            serversocket.close()
            if(SVMFeatureDataSet=="End"){
                reading=0
            }
            else{
                var DataSplit = SVMFeatureDataSet.split(",")
                writestring =  DataSplit(0) + " 1:" + DataSplit(1) + " 2:" + DataSplit(2)
                pw.write(writestring)
            }
        pw.close
        }
    }
}