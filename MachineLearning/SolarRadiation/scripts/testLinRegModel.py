# -*- coding: utf-8 -*-
"""
Created on Wed Jan 20 13:12:58 2016

@author: jesse
"""

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
myPath = "C:\Users\jesse\Downloads\Data1B-2B-3B\Data1B3B"

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

# split up tuples
for i in range(0, len(inputList)):
    inpList.append(inputList[i].split(",")[0])
    inpList.append(inputList[i].split(",")[1])
    inpList.append(inputList[i].split(",")[2])

# Break data up into each input chunk
for i in range(0, len(inpList), 1225*3):    
    val = inpList[i:i+1225*3]
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


# BEGIN VISUALIZATION
xxTest = []
yxTest = []
zxTest = []
# split values into x,y, and z components
for test in xTest:
    xxTest.append(test[::3])
    yxTest.append(test[1::3])
    zxTest.append(test[2::3])
    
# PLOT OUTPUTS
plt.scatter(xxTest, yTest,  color='black')
plt.plot(xxTest, regr.predict(xTest), color='blue',
         linewidth=3)

plt.xticks(())
plt.yticks(())

plt.show()

plt.scatter(yxTest, yTest,  color='black')
plt.plot(yxTest, regr.predict(xTest), color='blue',
         linewidth=3)

plt.xticks(())
plt.yticks(())

plt.show()

plt.scatter(zxTest, yTest,  color='black')
plt.plot(zxTest, regr.predict(xTest), color='blue',
         linewidth=3)

plt.xticks(())
plt.yticks(())

plt.show()



