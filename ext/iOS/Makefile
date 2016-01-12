SRCDIR = ./sdk
LIBOUTDIR = ../../Assets/Adjust/iOS

default: copy

build:	
	cd $(SRCDIR); \
	xcodebuild -target AdjustStatic -configuration Release

copy: build
	cp $(SRCDIR)/AdjustSdk.a $(LIBOUTDIR); \
	cp $(SRCDIR)/Headers/* $(LIBOUTDIR); \
	rm $(SRCDIR)/AdjustSdk.a; \
	rm -rf $(SRCDIR)/Headers
