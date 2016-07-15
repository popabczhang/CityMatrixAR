# -*- coding: utf-8 -*-
"""
Created on Wed Feb 17 10:11:27 2016

@author: jesse
"""

# -*- coding: utf-8 -*-
"""
Created on Wed Jan 20 13:12:58 2016

@author: jesse
"""

def trainLinReg(myPath = "C:/Users/jesse/Documents/Data45", outputFile = "C:/Users/jesse/Documents/Data45/regr.pkl"):
    # add ml libraries
    from sklearn import linear_model
    # add basic data munging
    from os import listdir
    # Properly represent data
    import numpy as np
    # commands for reading in files on the system
    from os.path import isfile, join
    # add plots
    import matplotlib.pyplot as plt
    
    # READ IN FILES
    
    inputList = []
    outputList = []
    
    # define the directory of the files
    
    
    # make a list of the files in the directory. 
    textFiles = [f for f in listdir(myPath) if isfile(join(myPath, f))]
    
    for tf in textFiles:
        # open a file and read the data
        f = open(join(myPath,tf))
        data = f.read()
        f.close()
        # identify sections of data
        splitData = data.split("\n[start]")
         
        buildingHeight = splitData[0].split("\n")
        buildingHeight = buildingHeight[1:(len(buildingHeight)-1)]
        
        seedNum = splitData[1].split("\n")
        seedNum = buildingHeight[1:(len(buildingHeight)-1)]
        
        inputData = splitData[2].split("\n")
        inputData = inputData[1:(len(inputData)-1)]
        
        diffData = splitData[3].split("\n")
        diffData = diffData[1:(len(diffData)-2)]
    
        inputList.extend(inputData)
        outputList.extend(diffData)
    
    #PREPARE DATA FOR ANALYSIS
    
    selectInput = []
    inpList = []
    selectOutput = []
    
    inpVals = ['0.1','25.0', '50.0', '80.0']
    newInpVals = ['1','2','3','4']
    # split up tuples
    for i in range(0, len(inputList)):
        inpList.append(inputList[i].split(",")[2])
    
    for j in range(len(inpVals)):
        for i in range(len(inpList)):
            if (inpList[i] == inpVals[j]):
                inpList[i] = newInpVals[j] 
        
        
    # Break data up into each input chunk
    for i in range(0, len(inpList), 1225):    
        val = inpList[i:i+1225]
        selectInput.append(val)    
    
    # split the data into training and testing sets
    xTrain = selectInput[:-50]
    xTest = selectInput[-50:]
    
    
    
    # change the type from a list to an array for use with 
    xTrain = np.array(xTrain).astype(np.float)
    xTest = np.array(xTest).astype(np.float)
    
    # Break data up into each ouput chunk
    for i in range(0, len(outputList), 1225):
        val = outputList[i:i+1225]
        selectOutput.append(val)
    
    # split the data into training and testing sets
    yTrain = selectOutput[:-50]
    yTest = selectOutput[-50:]
    
    # change the type from a list to an array for use with 
    yTrain = np.array(yTrain).astype(np.float)
    yTest = np.array(yTest).astype(np.float)
    
    # Use linear regression model
    regr = linear_model.LinearRegression()
    
    # Train the model using the training sets
    regr.fit(xTrain, yTrain)
    
    # The coefficients
    print('Coefficients: \n', regr.coef_)
    # The mean square error
    print("Residual sum of squares: %.2f"
          % np.mean((regr.predict(xTest) - yTest) ** 2))
    # Explained variance score: 1 is perfect prediction
    print('Variance score: %.2f' % regr.score(xTest, yTest))
    
    from sklearn.externals import joblib
    joblib.dump(regr, outputFile) 
import sys
#trainLinReg(sys.argv[1],sys.argv[2])
trainLinReg()