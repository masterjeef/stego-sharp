using StegoSharp.Enums;
using StegoSharp.ImagePropertyParsing.Interfaces;
using StegoSharp.ImagePropertyParsing.Strategies;
using StegoSharp.Models;
using System;
using System.Collections.Generic;

namespace StegoSharp.ImagePropertyParsing
{

    public class StegoImagePropertyParser
    {

        private readonly Dictionary<PropertyItemType, IImagePropertyParser> _parserLookup = new Dictionary<PropertyItemType, IImagePropertyParser>();

        public StegoImagePropertyParser()
        {

        }

        public string Parse(StegoImageProperty stegoImageProperty)
        {
            var propertyType = stegoImageProperty.PropertyItemType;

            if (_parserLookup.ContainsKey(propertyType))
            {
                return _parserLookup[propertyType].Parse(stegoImageProperty.Data);
            }

            IImagePropertyParser parsingStrategy = null;

            switch (propertyType)
            {
                case PropertyItemType.ByteArray :
                case PropertyItemType.Custom :
                    parsingStrategy = new ByteArrayImagePropertyParser();
                    break;
                case PropertyItemType.UnsignedShortArray :
                    parsingStrategy = new UShortArrayImagePropertyParser();
                    break;
                case PropertyItemType.UnsignedLongArray :
                case PropertyItemType.UnsignedLongFractionArray :
                    parsingStrategy = new ULongArrayImagePropertyParser();
                    break;
                case PropertyItemType.SignedLongArray :
                case PropertyItemType.SignedLongFractionArray :
                    parsingStrategy = new LongArrayImagePropertyParser();
                    break;
                case PropertyItemType.String:
                    parsingStrategy = new StringImagePropertyParser();
                    break;
                default :
                    throw new ArgumentOutOfRangeException(string.Format("Type {0} not handled", propertyType));
            }

            _parserLookup[propertyType] = parsingStrategy;

            return Parse(stegoImageProperty);
        }

    }
}
