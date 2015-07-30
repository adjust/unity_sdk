SRCDIR = ./sdk
LIBOUTDIR = ../../Assets/Adjust/iOS

default: copy

build:	
	cd $(SRCDIR); \
	xcodebuild -target framework -configuration Release

copy: build
	cp $(SRCDIR)/Adjust.a $(LIBOUTDIR); \
	rm $(SRCDIR)/Adjust.a
