# Script to extract training data and train a machine learning algorithm on that data, outputting the model information file to the given directory
# Author: Alex "Alxe" Aubuchon

OUTDIR="$(cd "$1"; pwd)"
OUTNAME="solar-linear.txt"

# Check if all arguments properly supplied
if [[ "$OUTDIR" == "" || ! -d "$OUTDIR"  ]]; then
  echo "No outdir specified. Usage: ./train.sh [Regression Output Directory]"
  exit 1
fi

# Change to the proper working directory
PREV_DIR="$(pwd)"
cd "$(dirname "${BASH_SOURCE}")"

ZIP_DIR="./data"
RAW_DIR="./data/raw"
SCRIPTS_DIR="./scripts"
TRAIN_SCRIPT="$SCRIPTS_DIR/trainLinRegModel.py"

# Extract data if there are no files in RAW_DIR
if [[ !(-d "$RAW_DIR" && $(ls -A "$RAW_DIR")) ]]; then
  echo "No data files. Extracting from available .zip files..."
  for file in $(ls -A $ZIP_DIR); do
    if [[ "${file: -4}" == ".zip" ]]; then
      echo "$file"
      unzip -jqo "$ZIP_DIR/$file" -d "$RAW_DIR"
    fi
  done
fi


# Run the training script
python "$TRAIN_SCRIPT" "$RAW_DIR" "$OUTDIR/$OUTNAME"

cd "$PREV_DIR"

exit 0
