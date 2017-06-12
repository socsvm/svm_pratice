/*****************************************************************************
* Filename:          C:\Users\Student\project_2\project_2.srcs\sources_1\edk\system/drivers/svm_v1_00_a/src/svm.h
* Version:           1.00.a
* Description:       svm Driver Header File
* Date:              Mon Jun 05 13:53:03 2017 (by Create and Import Peripheral Wizard)
*****************************************************************************/

#ifndef SVM_H
#define SVM_H

/***************************** Include Files *******************************/

#include "xil_types.h"
#include "xstatus.h"
#include "xil_io.h"

/************************** Constant Definitions ***************************/


/**
 * User Logic Slave Space Offsets
 * -- SLV_REG0 : user logic slave module register 0
 * -- SLV_REG1 : user logic slave module register 1
 * -- SLV_REG2 : user logic slave module register 2
 */
#define SVM_USER_SLV_SPACE_OFFSET (0x00000000)
#define SVM_SLV_REG0_OFFSET (SVM_USER_SLV_SPACE_OFFSET + 0x00000000)
#define SVM_SLV_REG1_OFFSET (SVM_USER_SLV_SPACE_OFFSET + 0x00000004)
#define SVM_SLV_REG2_OFFSET (SVM_USER_SLV_SPACE_OFFSET + 0x00000008)

/**************************** Type Definitions *****************************/


/***************** Macros (Inline Functions) Definitions *******************/

/**
 *
 * Write a value to a SVM register. A 32 bit write is performed.
 * If the component is implemented in a smaller width, only the least
 * significant data is written.
 *
 * @param   BaseAddress is the base address of the SVM device.
 * @param   RegOffset is the register offset from the base to write to.
 * @param   Data is the data written to the register.
 *
 * @return  None.
 *
 * @note
 * C-style signature:
 * 	void SVM_mWriteReg(u32 BaseAddress, unsigned RegOffset, u32 Data)
 *
 */
#define SVM_mWriteReg(BaseAddress, RegOffset, Data) \
 	Xil_Out32((BaseAddress) + (RegOffset), (u32)(Data))

/**
 *
 * Read a value from a SVM register. A 32 bit read is performed.
 * If the component is implemented in a smaller width, only the least
 * significant data is read from the register. The most significant data
 * will be read as 0.
 *
 * @param   BaseAddress is the base address of the SVM device.
 * @param   RegOffset is the register offset from the base to write to.
 *
 * @return  Data is the data from the register.
 *
 * @note
 * C-style signature:
 * 	u32 SVM_mReadReg(u32 BaseAddress, unsigned RegOffset)
 *
 */
#define SVM_mReadReg(BaseAddress, RegOffset) \
 	Xil_In32((BaseAddress) + (RegOffset))


/**
 *
 * Write/Read 32 bit value to/from SVM user logic slave registers.
 *
 * @param   BaseAddress is the base address of the SVM device.
 * @param   RegOffset is the offset from the slave register to write to or read from.
 * @param   Value is the data written to the register.
 *
 * @return  Data is the data from the user logic slave register.
 *
 * @note
 * C-style signature:
 * 	void SVM_mWriteSlaveRegn(u32 BaseAddress, unsigned RegOffset, u32 Value)
 * 	u32 SVM_mReadSlaveRegn(u32 BaseAddress, unsigned RegOffset)
 *
 */
#define SVM_mWriteSlaveReg0(BaseAddress, RegOffset, Value) \
 	Xil_Out32((BaseAddress) + (SVM_SLV_REG0_OFFSET) + (RegOffset), (u32)(Value))
#define SVM_mWriteSlaveReg1(BaseAddress, RegOffset, Value) \
 	Xil_Out32((BaseAddress) + (SVM_SLV_REG1_OFFSET) + (RegOffset), (u32)(Value))
#define SVM_mWriteSlaveReg2(BaseAddress, RegOffset, Value) \
 	Xil_Out32((BaseAddress) + (SVM_SLV_REG2_OFFSET) + (RegOffset), (u32)(Value))

#define SVM_mReadSlaveReg0(BaseAddress, RegOffset) \
 	Xil_In32((BaseAddress) + (SVM_SLV_REG0_OFFSET) + (RegOffset))
#define SVM_mReadSlaveReg1(BaseAddress, RegOffset) \
 	Xil_In32((BaseAddress) + (SVM_SLV_REG1_OFFSET) + (RegOffset))
#define SVM_mReadSlaveReg2(BaseAddress, RegOffset) \
 	Xil_In32((BaseAddress) + (SVM_SLV_REG2_OFFSET) + (RegOffset))

/************************** Function Prototypes ****************************/


/**
 *
 * Run a self-test on the driver/device. Note this may be a destructive test if
 * resets of the device are performed.
 *
 * If the hardware system is not built correctly, this function may never
 * return to the caller.
 *
 * @param   baseaddr_p is the base address of the SVM instance to be worked on.
 *
 * @return
 *
 *    - XST_SUCCESS   if all self-test code passed
 *    - XST_FAILURE   if any self-test code failed
 *
 * @note    Caching must be turned off for this function to work.
 * @note    Self test may fail if data memory and device are not on the same bus.
 *
 */
XStatus SVM_SelfTest(void * baseaddr_p);
/**
*  Defines the number of registers available for read and write*/
#define TEST_AXI_LITE_USER_NUM_REG 3


#endif /** SVM_H */
