# Variables
TEXFILE = stage1
PDFVIEWER = evince

# Default target to generate the PDF
all: $(TEXFILE).pdf

# Rule to generate the PDF
$(TEXFILE).pdf: $(TEXFILE).tex
	pdflatex $(TEXFILE).tex
	pdflatex $(TEXFILE).tex  # Run twice for proper references

# Clean auxiliary files, but keep the PDF
clean:
	rm -f *.aux *.log *.out *.toc *.synctex.gz

# Clean everything including the PDF
cleanall: clean
	rm -f $(TEXFILE).pdf

# Open the generated PDF
view: $(TEXFILE).pdf
	$(PDFVIEWER) $(TEXFILE).pdf &