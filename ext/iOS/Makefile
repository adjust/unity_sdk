SRCDIR = ./sdk
LIBOUTDIR = ../../Assets/Plugins/iOS

default: copy

build:	
	cd $(SRCDIR); \
	xcodebuild -target framework -configuration Release

copy: build
	cp $(SRCDIR)/Adjust.a $(LIBOUTDIR); \
	cp $(SRCDIR)/Headers/* $(LIBOUTDIR); \
	rm $(SRCDIR)/Adjust.a; \
	rm -rf $(SRCDIR)/Headers
