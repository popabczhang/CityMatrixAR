# Script to automatically train all machine learning algorithms in sibling directories of this file and save the model files to the given directory.
# Author: Alex "Alxe" Aubuchon

OUTDIR="$1"
PARENT_DIR="./"
SCRIPT_NAME="train.sh"

if [[ "$OUTDIR" == "" || ! -d "$OUTDIR" ]]; then
  echo "No out directory specified. Usage: ./trainAll.sh [Regression Output Directory]"
  exit 1
fi

for dir in $(ls "$PARENT_DIR"); do
  if [[ -d "$PARENT_DIR/$dir" && -e "$PARENT_DIR/$dir/$SCRIPT_NAME" ]]; then
    echo "--- Training $dir"
    "$PARENT_DIR/$dir/$SCRIPT_NAME" "$OUTDIR"
  fi
done

exit 0
