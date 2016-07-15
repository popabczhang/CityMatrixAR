ZIP_DIR="./data"
RAW_DIR="./data/raw"
SCRIPTS_DIR="./scripts"
TRAIN_SCRIPT="$SCRIPTS_DIR/trainLinRegModel.py"
OUTFILE=$1

if [[ $OUTFILE == "" ]]; then
  echo "No outfile specified. Usage: ./train.sh [Regression Output File]"
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

python $TRAIN_SCRIPT $RAW_DIR $OUTFILE
