# -*- coding: utf-8 -*-
"""
Created on Wed Jan 20 13:12:58 2016

@author: Jesse Michel
Edited by Alex Aubuchon
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
# file IO
import sys

def getBuildingCell(str):
    data = str.split(',')
    buildingCell = heightFilter(data[2])
    return buildingCell

def heightFilter(height):
    inpVals = ['0.1','25.0', '50.0', '80.0']
    newInpVals = ['1','2','3','4']
    for i in range(len(inpVals)):
        if (height == inpVals[i]):
            return newInpVals[i]
    raise Exception('Encountered unexpected height: ' + height)

dataPath = sys.argv[1]
outputFile = sys.argv[2]

xData = []
yData = []

# make a list of the files in the directory.
textFiles = [f for f in listdir(dataPath) if isfile(join(dataPath, f))]

for tf in textFiles:
    # open a file and read the data
    f = open(join(dataPath,tf))
    data = f.read()
    f.close()
    
    # identify sections of data
    splitData = data.split("\n[start]")

    # get the central building height --not used
    buildingHeight = splitData[0].split("\n")
    buildingHeight = buildingHeight[1:(len(buildingHeight)-1)]

    # get the seed number --not used
    seedNum = splitData[1].split("\n")
    seedNum = seedNum[1:(len(seedNum)-1)]

    # get the building cell height data for this 5x5 building file
    inputData = splitData[2].split("\n")
    inputData = inputData[1:(len(inputData)-1)]
    buildingCellHeights = []
    for i in range(len(inputData)):
        buildingCellHeights.append(getBuildingCell(inputData[i]))

    # get the deltas for each building cell
    diffData = splitData[3].split("\n")
    diffData = diffData[1:(len(diffData)-2)]

    # add this 5x5 building set to the input and output arrays
    xData.append(buildingCellHeights)
    yData.append(diffData)

# split the input data into training and testing sets and convert to np float array
xTrain = np.array(xData[:-50]).astype(np.float)
xTest = np.array(xData[-50:]).astype(np.float)

# split the output data into training and testing sets and convert to np float array
yTrain = np.array(yData[:-50]).astype(np.float)
yTest = np.array(yData[-50:]).astype(np.float)

# Use linear regression model
regr = linear_model.LinearRegression()

# Train the model using the training sets
regr.fit(xTrain, yTrain)

# The mean square error
print("Residual sum of squares: %.2f"
      % np.mean((regr.predict(xTest) - yTest) ** 2))
# Explained variance score: 1 is perfect prediction
print('Variance score: %.2f' % regr.score(xTest, yTest))

out = open(outputFile, 'w')
for row in regr.coef_:
    for j in range(len(row)):
        if(j == 0):
            out.write("%s" % row[j])
        out.write(",%s" % row[j])
    out.write("\n")

exit(0)


    
    
