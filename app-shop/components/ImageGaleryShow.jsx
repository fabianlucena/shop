import { useState, useRef, useEffect } from 'react';
import { ScrollView, View, Platform, Pressable, Dimensions, Text } from 'react-native';
import ImageShow from './ImageShow.jsx';

const { width: screenWidth, height: screenHeight } = Dimensions.get('window');

export default function ImageGaleryShow({
  images,
  gap = 24,
  auto = false,
  interval = 3500,
  containerStyle = {},
  canFullScreen = true,
  onPress,
  ...props
}) {
  const ref = useRef(null);
  const [scrollWidth, setScrollWidth] = useState(0);
  const scrollRef = useRef(null);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [isFullScreen, setIsFullScreen] = useState(false);

  useEffect(() => {
    scrollRef.current?.scrollTo({
      x: 0,
      animated: true,
    });

    const _interval = setInterval(() => {
      if (!isFullScreen)
        next();
    }, interval);
    return () => clearInterval(_interval);
  }, [auto, interval, scrollWidth, images?.length, gap]);

  function next() {
    setCurrentIndex(prev => {
      const nextIndex = (prev + 1) % images?.length;
      scrollRef.current?.scrollTo({
        x: nextIndex * (scrollWidth + gap),
        animated: true,
      });
      return nextIndex;
    });
  }

  function toggleFullScreen() {
    setIsFullScreen(prev => !prev);
  }

  return <View
      ref={ref}
      onLayout={event => {
        if (Platform.OS !== 'web') {
          setScrollWidth(Math.floor(event.nativeEvent.layout.width));
        }

        if (Platform.OS === 'web' && ref.current) {
          const measuredWidth = ref.current?.parentNode?.parentNode?.clientWidth;
          if (measuredWidth) {
            const computed = window.getComputedStyle(ref.current.parentNode.parentNode);
            const margin = {
              top: parseInt(computed.marginTop),
              right: parseInt(computed.marginRight),
              bottom: parseInt(computed.marginBottom),
              left: parseInt(computed.marginLeft),
            };
            padding = {
              top: parseInt(computed.paddingTop),
              right: parseInt(computed.paddingRight),
              bottom: parseInt(computed.paddingBottom),
              left: parseInt(computed.paddingLeft),
            };
            
            setScrollWidth(measuredWidth - margin.left - margin.right - padding.left - padding.right);
          }
        }
      }}
      style={{
        width: Platform.OS === 'web' ? scrollWidth : null,
        ...containerStyle,
        ...(isFullScreen ? {
          position: 'absolute',
          top: 0,
          left: 0,
          right: 0,
          bottom: 0,
          zIndex: 1000,
          width: screenWidth,
          height: screenHeight,
          margin: 0,
        } : {}),
        flex: 1,
      }}
    >
      <Pressable
        onPress={() => {
          if (canFullScreen) {
            if (isFullScreen)
              next();
            else
              setIsFullScreen(true);
          } else if (onPress) {
            onPress();
          } else {
            next();
          }
        }}
      >
        {isFullScreen && <View
          style={{
            width: 24,
            right: 8,
            top: 8,
            borderRadius: 8,
            zIndex: 1001,
            position: 'absolute',
            backgroundColor: '#d0d0d040',
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
          }}
        >
          <Pressable
            onPress={() => setIsFullScreen(false)}
          >
            <Text
              style={{
                color: '#606060',
                fontSize: 18,
                fontWeight: 'bold',
              }}
            >
              X
            </Text>
          </Pressable>
        </View> || null}
        <ScrollView
          ref={scrollRef}
          horizontal
          style={{
            overflow: 'scroll',
          }}
          contentContainerStyle={{
            gap,
          }}
        >
          {images?.map((image, index) => (
            <ImageShow
              key={index}
              {...props}
              {...image}
              style={{
                width: scrollWidth,
                ...props.style,
                ...image.style,
              }}
            />
          )) || null}
        </ScrollView>
        <View
          style={{
            flex: 0,
            position: 'relative',
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            backgroundColor: 'red',
          }}
        >
          <View
            style={{
              position: 'absolute',
              display: 'flex',
              flexDirection: 'row',
              justifyContent: 'center',
              alignItems: 'center',
              marginTop: -24,
            }}
          >
            {images?.map((image, index) => (
              <View
                key={index}
                style={{
                  width: 8,
                  height: 8,
                  margin: 2,
                  borderWidth: 1,
                  borderColor: '#ccc8',
                  backgroundColor: index === currentIndex ? '#aaa8' : 'transparent',
                  borderRadius: 3,
                }}
              />
            )) ?? null}
          </View>
        </View>
      </Pressable>
    </View>;
}
