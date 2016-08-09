# -*- coding: utf-8 -*-
"""
Created on Mon Aug 08 17:09:20 2016

@author: Alex
"""

# add basic data munging
from os import listdir
# commands for reading in files on the system
from os.path import isfile, join
# file IO
import sys

def heightFilter(height):
    inpVals = ['0.1','25.0', '50.0', '80.0']
    newInpVals = ['1','2','3','4']
    for i in range(len(inpVals)):
        if (height == inpVals[i]):
            return newInpVals[i]
    raise Exception('Encountered unexpected height: ' + height)

def getNewBuildingData(old):
    centralBuilding = "2,2," + heightFilter(old[0].split(",")[2])
    out = [""]*25
    out[12] = centralBuilding
    counter = 49
    for i in range(0, 5):
        for j in range(4, -1, -1):
            if(i * 5 + j != 12):
                out[i * 5 + j] = str(i) + "," + str(j) + "," + heightFilter(old[counter].split(",")[2])
                counter += 49
    return out
    

def getNewDiffData(old):
    out = [""]*1225
    counter = 0
    for k in range(0, 7):
        for l in range(6, -1, -1):
            out[(2 * 49 * 5) + (2 * 49) + (k * 7) + l] = old[counter]
            counter += 1
    for i in range(0, 5):
        for j in range(4, -1, -1):
            if((i == 2) and (j == 2)):
                tmp = counter
                counter = 0
            for k in range(0, 7):
                for l in range(6, -1, -1):
                    out[(i * 49 * 5) + (j * 49) + (k * 7) + l] = old[counter]
                    counter += 1
            if((i == 2) and (j == 2)):
                counter = tmp
    return out
    


dataPath = sys.argv[1]

textFiles = [f for f in listdir(dataPath) if isfile(join(dataPath, f))]

for tf in textFiles:
    # open a file and read the data
    f = open(join(dataPath,tf), 'r')
    data = f.read()
    f.close()    
    
    # identify sections of data
    splitData = data.split("\n[start]")

    # get the central building height --not used
    buildingHeight = splitData[0].split("\n")
    buildingHeight = buildingHeight[1]

    # get the seed number --not used
    seedNum = splitData[1].split("\n")
    seedNum = seedNum[1]

    # get the building cell height data for this 5x5 building file
    inputData = splitData[2].split("\n")
    inputData = inputData[1:(len(inputData)-1)]
    newBuildingData = getNewBuildingData(inputData)
    
    # get the deltas for each building cell
    diffData = splitData[3].split("\n")
    diffData = diffData[1:(len(diffData)-2)]
    newDiffData = getNewDiffData(diffData)
    
    f = open(join(dataPath,tf), 'w')
    f.write("[start]central building height\n" + heightFilter(buildingHeight) + "\n[end]central building height\n")
    f.write("[start]random seed number\n" + seedNum + "\n[end]random seed number\n")
    f.write("[start]input node coordination\n")
    for b in newBuildingData:
        f.write(str(b) + "\n")
    f.write("[end]input node coordination\n")
    f.write("[start]node reads difference\n")
    for d in newDiffData:
        f.write(str(d) + "\n")
    f.write("[end]node reads difference\n")
    
    