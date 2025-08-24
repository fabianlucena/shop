import { useState, useRef, useEffect } from 'react';
import { ScrollView, View, Platform } from 'react-native';
import ImageShow from './ImageShow.jsx';

export default function ImageGaleryShow({
  images,
  gap = 24,
  auto = false,
  interval = 3500,
  ...props
}) {
  const ref = useRef(null);
  const [scrollWidth, setScrollWidth] = useState(0);
  const scrollRef = useRef(null);
  const [currentIndex, setCurrentIndex] = useState(0);

  useEffect(() => {
    scrollRef.current?.scrollTo({
      x: 0,
      animated: true,
    });

    const _interval = setInterval(next, interval);
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
      }}
    >
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
    </View>;
}
