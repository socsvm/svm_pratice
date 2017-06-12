
module SVMmodule
/*
......
*/
wire [15:0]newSupportVector;
wire [7:0]Supportvector1;
wire [7:0]Supportvector2;
output result;

wire [13:0]FeatureVector;
wire [6:0]Vector1;
wire [6:0]Vector2;
  // USER logic implementation added here
assign newSupportVector = slv_reg0;
assign FeatureVector = slv_reg2;
SupportVectorTemp temp (newSupportVector,Supportvector1,Supportvector2);
VectorTemp temp2 (FeatureVector,Vector1,Vector2);
SVMRecall recall(Supportvector1,Supportvector2,Vector1,Vector2,result);

/*
......
*/
endmodule

module SupportVectorTemp(newSupportVector,SupportVector1,SupportVector2);
input [15:0]newSupportVector;
output reg [7:0]SupportVector1,SupportVector2;

always@(newSupportVector)
begin
	SupportVector1<=newSupportVector[15:8];
	SupportVector2<=newSupportVector[7:0];
end
endmodule

module VectorTemp(newVector,Vector1,Vector2);
input [13:0]newVector;
output reg [6:0]Vector1,Vector2;

always@(newVector)
begin
	Vector1<=newVector[13:7];
	Vector2<=newVector[6:0];
end
endmodule

module SVMRecall(vector1,vertor2,SupportVector1,SupportVector2,result);
input[6:0] vector1;
input[6:0] vertor2;
input[7:0] SupportVector1;
input[7:0] SupportVector2;
output result;
reg [15:0] svmrecallone;
reg [15:0] svmrecalltwo;
reg [15:0] svmrecallresult;

always@(*)
begin
	//向量內積
	svmrecallone <= vector1 * SupportVector1[6:0];
	svmrecallone[15]<=SupportVector1[7];
	svmrecalltwo <= vertor2 * SupportVector2[6:0];
	svmrecalltwo[15]<=SupportVector2[7];
	
	if(svmrecallone[15] == 1 && svmrecalltwo[15]==1) //兩個都負
		begin
			svmrecallresult[14:0]<=svmrecallone[14:0]+svmrecalltwo[14:0];
			svmrecallresult[15]<=1;
		end
	else if(svmrecallone[15] == 1 && svmrecalltwo[15]==0) //前負後正
		begin
			if(svmrecallone[14:0] > svmrecalltwo[14:0])
				begin
				svmrecallresult[14:0]<=svmrecallone[14:0]-svmrecalltwo[14:0];
				svmrecallresult[15]<=1;
				end
			else
				begin
				svmrecallresult[14:0]<=svmrecalltwo[14:0]-svmrecallone[14:0];
				svmrecallresult[15]<=0;
				end
		end
	else if(svmrecallone[15] == 0 && svmrecalltwo[15]==1) //後正前負
		begin
			if(svmrecallone[14:0] > svmrecalltwo[14:0])
				begin
				svmrecallresult[14:0]<=svmrecallone[14:0]-svmrecalltwo[14:0];
				svmrecallresult[15]<=0;
				end
			else
				begin
				svmrecallresult[14:0]<=svmrecalltwo[14:0]-svmrecallone[14:0];
				svmrecallresult[15]<=1;
				end
		end
	else //兩個都正
		begin
			svmrecallresult[14:0]<=svmrecallone[14:0]+svmrecalltwo[14:0];
			svmrecallresult[15]<=0;
		end
	
	if(svmrecallresult[15]==1)
		result<=0;
	else
		result<=1;
end

endmodule
