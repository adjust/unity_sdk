SRCDIR = ./sdk
LIBOUTDIR = ../../Assets/Adjust/iOS
STATICFRAMEWORK = $(SRCDIR)/Frameworks/Static/AdjustSdk.framework

default: copy

build:	
	cd $(SRCDIR); \
	xcodebuild -target AdjustStatic -configuration Release

copy: build
	cp $(STATICFRAMEWORK)/Versions/A/AdjustSdk $(LIBOUTDIR)/AdjustSdk.a; \
	cp $(STATICFRAMEWORK)/Versions/A/Headers/* $(LIBOUTDIR);