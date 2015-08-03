MVNDIR = ./sdk/Adjust
JARINDIR = ./sdk/Adjust/target
JAROUTDIR = ../../Assets/Adjust/Android

default: copy

build: clean
	cd $(MVNDIR); mvn package

clean:
	cd $(MVNDIR); mvn clean

copy: build
	rm $(JAROUTDIR)/adjust-android*; \
	cp $(JARINDIR)/adjust-android-*.*.*.jar $(JAROUTDIR); \
	rm $(JAROUTDIR)/*-javadoc.jar; \
	rm $(JAROUTDIR)/*-sources.jar; \
	mv $(JAROUTDIR)/adjust-android-*.*.*.jar $(JAROUTDIR)/adjust-android.jar
