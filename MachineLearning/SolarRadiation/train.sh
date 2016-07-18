# Script to extract training data and train a machine learning algorithm on that data, outputting the model information file to the given directory
# Author: Alex "Alxe" Aubuchon

ZIP_DIR="./data"
RAW_DIR="./data/raw"
SCRIPTS_DIR="./scripts"
TRAIN_SCRIPT="$SCRIPTS_DIR/trainLinRegModel.py"
OUTDIR=$1
OUTNAME="solar-linear.regr"

if [[ $OUTFILE == "" ]]; then
  echo "No outfile specified. Usage: ./train.sh [Regression Output Directory]"
  exit 1
fi

if [[ !(-d $RAW_DIR && $(ls -A $RAW_DIR)) ]]; then
  echo "No data files. Extracting from available .zip files..."
  for file in $(ls $ZIP_DIR); do
    if [[ ${file: -4} == ".zip" ]]; then
    echo $file
    unzip -jqo "$DATA_DIR/$file" -d $RAW_DIR
    fi
  done
fi

python $TRAIN_SCRIPT $RAW_DIR "$OUTDIR/$OUTNAME"

exit 0
