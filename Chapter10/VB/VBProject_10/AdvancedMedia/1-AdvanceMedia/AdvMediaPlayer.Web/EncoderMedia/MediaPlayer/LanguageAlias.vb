Imports System
Imports System.Collections.ObjectModel
Imports System.Diagnostics
Imports System.Globalization
Imports System.Text

' Namespace ExpressionMediaPlayer

    Dim internal Class LanguageAlias
        ''' <summary>
        ''' converts an Iso three letter language id to the equivalent two letter code
        ''' </summary>
        ''' <param name="threeLetter"></param>
        ''' <returns></returns>
        internal static string IsoThreeLetterToIsoTwoLetter(string threeLetter)

            If (string.IsNullOrEmpty(threeLetter)) Then
                return string.Empty
            End If

            Debug.Assert(3 = threeLetter.Length, "Expected three letter ISO 639 language code")
            InitCultureTable()

            If (cultureTable  IsNot Nothing) Then

                Dim iso639Three As String  = threeLetter.ToLowerInvariant()


                For Each var cultureTableEntry in cultureTable


                    If (0 = string.CompareOrdinal(cultureTableEntry.ThreeLetterCode, iso639Three)) Then
                        return cultureTableEntry.TwoLetterCode
                    End If
                Next    '   var
            End If

            return string.Empty
        }

        ''' <summary>
        ''' converts an Iso two letter language id to the equivalent three letter code
        ''' </summary>
        ''' <param name="twoLetter"></param>
        ''' <returns></returns>
        internal static string IsoTwoLetterToIsoThreeLetter(string twoLetter)
        {

            If (string.IsNullOrEmpty(twoLetter)) Then
                return string.Empty
            End If

            Debug.Assert(2 = twoLetter.Length, "Expected two letter ISO 639-1 language code")
            InitCultureTable()

            If (cultureTable  IsNot Nothing) Then

                Dim iso639Two As String  = twoLetter.ToLowerInvariant()


                For Each var cultureTableEntry in cultureTable


                    If (0 = string.CompareOrdinal(cultureTableEntry.TwoLetterCode, iso639Two)) Then
                        return cultureTableEntry.ThreeLetterCode
                    End If
                Next    '   var
            End If

            return string.Empty
        }

        ''' <summary>
        ''' lookup table entry
        ''' </summary>
        Private Class CultureTableEntry
        {
            internal string TwoLetterCode { get; private set; }
            internal string ThreeLetterCode { get; private set; }
            internal CultureTableEntry(string twoLetterCode, string threeLetterCode)
            {
                Me.TwoLetterCode = twoLetterCode
                Me.ThreeLetterCode = threeLetterCode
            }
        End Class   '   CultureTableEntry


        ''' <summary>
        ''' The lookup table
        ''' </summary>
        End Property


        ''' <summary>
        ''' method to initialize the lookup table
        ''' </summary>
        End Property



            If (cultureTable Is Nothing) Then
                cultureTable = New Collection<CultureTableEntry>()

                '  "af" - "Afrikaans"
                cultureTable.Add(new CultureTableEntry("af", "afr"))
                '  "am" - "Amharic"
                cultureTable.Add(new CultureTableEntry("am", "amh"))
                '  "ar" - "Arabic"
                cultureTable.Add(new CultureTableEntry("ar", "ara"))
                '  "arn" - "Mapudungun"
                cultureTable.Add(new CultureTableEntry("arn", "arn"))
                '  "as" - "Assamese"
                cultureTable.Add(new CultureTableEntry("as", "asm"))
                '  "az-Cyrl" - "Azeri (Cyrillic)"
                cultureTable.Add(new CultureTableEntry("az", "aze"))
                '  "az-Latn" - "Azeri (Latin)"
                cultureTable.Add(new CultureTableEntry("az", "aze"))
                '  "az" - "Azeri"
                cultureTable.Add(new CultureTableEntry("az", "aze"))
                '  "ba" - "Bashkir"
                cultureTable.Add(new CultureTableEntry("ba", "bak"))
                '  "be" - "Belarusian"
                cultureTable.Add(new CultureTableEntry("be", "bel"))
                '  "bg" - "Bulgarian"
                cultureTable.Add(new CultureTableEntry("bg", "bul"))
                '  "bn" - "Bengali"
                cultureTable.Add(new CultureTableEntry("bn", "bng"))
                '  "bo" - "Tibetan"
                cultureTable.Add(new CultureTableEntry("bo", "bod"))
                '  "br" - "Breton"
                cultureTable.Add(new CultureTableEntry("br", "bre"))
                '  "bs-Latn" - "Bosnian (Latin)"
                cultureTable.Add(new CultureTableEntry("bs", "bsb"))
                '  "bs" - "Bosnian"
                cultureTable.Add(new CultureTableEntry("bs", "bsb"))
                '  "bs-Cyrl" - "Bosnian (Cyrillic)"
                cultureTable.Add(new CultureTableEntry("bs", "bsc"))
                '  "ca" - "Catalan"
                cultureTable.Add(new CultureTableEntry("ca", "cat"))
                '  "co" - "Corsican"
                cultureTable.Add(new CultureTableEntry("co", "cos"))
                '  "cs" - "Czech"
                cultureTable.Add(new CultureTableEntry("cs", "ces"))
                '  "cy" - "Welsh"
                cultureTable.Add(new CultureTableEntry("cy", "cym"))
                '  "da" - "Danish"
                cultureTable.Add(new CultureTableEntry("da", "dan"))
                '  "de" - "German"
                cultureTable.Add(new CultureTableEntry("de", "deu"))
                '  "dsb" - "Lower Sorbian"
                cultureTable.Add(new CultureTableEntry("dsb", "dsb"))
                '  "dv" - "Divehi"
                cultureTable.Add(new CultureTableEntry("dv", "div"))
                '  "el" - "Greek"
                cultureTable.Add(new CultureTableEntry("el", "ell"))
                '  "en" - "English"
                cultureTable.Add(new CultureTableEntry("en", "eng"))
                '  "es" - "Spanish"
                cultureTable.Add(new CultureTableEntry("es", "spa"))
                '  "et" - "Estonian"
                cultureTable.Add(new CultureTableEntry("et", "est"))
                '  "eu" - "Basque"
                cultureTable.Add(new CultureTableEntry("eu", "eus"))
                '  "fa" - "Persian"
                cultureTable.Add(new CultureTableEntry("fa", "fas"))
                '  "fi" - "Finnish"
                cultureTable.Add(new CultureTableEntry("fi", "fin"))
                '  "fil" - "Filipino"
                cultureTable.Add(new CultureTableEntry("fil", "fil"))
                '  "fo" - "Faroese"
                cultureTable.Add(new CultureTableEntry("fo", "fao"))
                '  "fr" - "French"
                cultureTable.Add(new CultureTableEntry("fr", "fra"))
                '  "fy" - "Frisian"
                cultureTable.Add(new CultureTableEntry("fy", "fry"))
                '  "ga" - "Irish"
                cultureTable.Add(new CultureTableEntry("ga", "gle"))
                '  "gd" - "Scottish Gaelic"
                cultureTable.Add(new CultureTableEntry("gd", "gla"))
                '  "gl" - "Galician"
                cultureTable.Add(new CultureTableEntry("gl", "glg"))
                '  "gsw" - "Alsatian"
                cultureTable.Add(new CultureTableEntry("gsw", "gsw"))
                '  "gu" - "Gujarati"
                cultureTable.Add(new CultureTableEntry("gu", "guj"))
                '  "ha-Latn" - "Hausa (Latin)"
                cultureTable.Add(new CultureTableEntry("ha", "hau"))
                '  "ha" - "Hausa"
                cultureTable.Add(new CultureTableEntry("ha", "hau"))
                '  "he" - "Hebrew"
                cultureTable.Add(new CultureTableEntry("he", "heb"))
                '  "hi" - "Hindi"
                cultureTable.Add(new CultureTableEntry("hi", "hin"))
                '  "hr" - "Croatian"
                cultureTable.Add(new CultureTableEntry("hr", "hrv"))
                '  "hsb" - "Upper Sorbian"
                cultureTable.Add(new CultureTableEntry("hsb", "hsb"))
                '  "hu" - "Hungarian"
                cultureTable.Add(new CultureTableEntry("hu", "hun"))
                '  "hy" - "Armenian"
                cultureTable.Add(new CultureTableEntry("hy", "hye"))
                '  "id" - "Indonesian"
                cultureTable.Add(new CultureTableEntry("id", "ind"))
                '  "ig" - "Igbo"
                cultureTable.Add(new CultureTableEntry("ig", "ibo"))
                '  "ii" - "Yi"
                cultureTable.Add(new CultureTableEntry("ii", "iii"))
                '  "is" - "Icelandic"
                cultureTable.Add(new CultureTableEntry("is", "isl"))
                '  "it" - "Italian"
                cultureTable.Add(new CultureTableEntry("it", "ita"))
                '  "iu-Cans" - "Inuktitut (Syllabics)"
                cultureTable.Add(new CultureTableEntry("iu", "iku"))
                '  "iu-Latn" - "Inuktitut (Latin)"
                cultureTable.Add(new CultureTableEntry("iu", "iku"))
                '  "iu" - "Inuktitut"
                cultureTable.Add(new CultureTableEntry("iu", "iku"))
                '  "" - "Invariant Language (Invariant Country)"
                cultureTable.Add(new CultureTableEntry("iv", "ivl"))
                '  "ja" - "Japanese"
                cultureTable.Add(new CultureTableEntry("ja", "jpn"))
                '  "ka" - "Georgian"
                cultureTable.Add(new CultureTableEntry("ka", "kat"))
                '  "kk" - "Kazakh"
                cultureTable.Add(new CultureTableEntry("kk", "kaz"))
                '  "kl" - "Greenlandic"
                cultureTable.Add(new CultureTableEntry("kl", "kal"))
                '  "km" - "Khmer"
                cultureTable.Add(new CultureTableEntry("km", "khm"))
                '  "kn" - "Kannada"
                cultureTable.Add(new CultureTableEntry("kn", "kan"))
                '  "ko" - "Korean"
                cultureTable.Add(new CultureTableEntry("ko", "kor"))
                '  "kok" - "Konkani"
                cultureTable.Add(new CultureTableEntry("kok", "kok"))
                '  "ky" - "Kyrgyz"
                cultureTable.Add(new CultureTableEntry("ky", "kir"))
                '  "lb" - "Luxembourgish"
                cultureTable.Add(new CultureTableEntry("lb", "ltz"))
                '  "lo" - "Lao"
                cultureTable.Add(new CultureTableEntry("lo", "lao"))
                '  "lt" - "Lithuanian"
                cultureTable.Add(new CultureTableEntry("lt", "lit"))
                '  "lv" - "Latvian"
                cultureTable.Add(new CultureTableEntry("lv", "lav"))
                '  "mi" - "Maori"
                cultureTable.Add(new CultureTableEntry("mi", "mri"))
                '  "mk" - "Macedonian (FYROM)"
                cultureTable.Add(new CultureTableEntry("mk", "mkd"))
                '  "ml" - "Malayalam"
                cultureTable.Add(new CultureTableEntry("ml", "mym"))
                '  "mn-Cyrl" - "Mongolian (Cyrillic)"
                cultureTable.Add(new CultureTableEntry("mn", "mon"))
                '  "mn-Mong" - "Mongolian (Traditional Mongolian)"
                cultureTable.Add(new CultureTableEntry("mn", "mon"))
                '  "mn" - "Mongolian"
                cultureTable.Add(new CultureTableEntry("mn", "mon"))
                '  "moh" - "Mohawk"
                cultureTable.Add(new CultureTableEntry("moh", "moh"))
                '  "mr" - "Marathi"
                cultureTable.Add(new CultureTableEntry("mr", "mar"))
                '  "ms" - "Malay"
                cultureTable.Add(new CultureTableEntry("ms", "msa"))
                '  "mt" - "Maltese"
                cultureTable.Add(new CultureTableEntry("mt", "mlt"))
                '  "nb" - "Norwegian (Bokmål)"
                cultureTable.Add(new CultureTableEntry("nb", "nob"))
                '  "no" - "Norwegian"
                cultureTable.Add(new CultureTableEntry("nb", "nob"))
                '  "ne" - "Nepali"
                cultureTable.Add(new CultureTableEntry("ne", "nep"))
                '  "nl" - "Dutch"
                cultureTable.Add(new CultureTableEntry("nl", "nld"))
                '  "nn" - "Norwegian (Nynorsk)"
                cultureTable.Add(new CultureTableEntry("nn", "nno"))
                '  "nso" - "Sesotho sa Leboa"
                cultureTable.Add(new CultureTableEntry("nso", "nso"))
                '  "oc" - "Occitan"
                cultureTable.Add(new CultureTableEntry("oc", "oci"))
                '  "or" - "Oriya"
                cultureTable.Add(new CultureTableEntry("or", "ori"))
                '  "pa" - "Punjabi"
                cultureTable.Add(new CultureTableEntry("pa", "pan"))
                '  "pl" - "Polish"
                cultureTable.Add(new CultureTableEntry("pl", "pol"))
                '  "prs" - "Dari"
                cultureTable.Add(new CultureTableEntry("prs", "prs"))
                '  "ps" - "Pashto"
                cultureTable.Add(new CultureTableEntry("ps", "pus"))
                '  "pt" - "Portuguese"
                cultureTable.Add(new CultureTableEntry("pt", "por"))
                '  "qut" - "K'iche"
                cultureTable.Add(new CultureTableEntry("qut", "qut"))
                '  "quz" - "Quechua"
                cultureTable.Add(new CultureTableEntry("quz", "qub"))
                '  "rm" - "Romansh"
                cultureTable.Add(new CultureTableEntry("rm", "roh"))
                '  "ro" - "Romanian"
                cultureTable.Add(new CultureTableEntry("ro", "ron"))
                '  "ru" - "Russian"
                cultureTable.Add(new CultureTableEntry("ru", "rus"))
                '  "rw" - "Kinyarwanda"
                cultureTable.Add(new CultureTableEntry("rw", "kin"))
                '  "sa" - "Sanskrit"
                cultureTable.Add(new CultureTableEntry("sa", "san"))
                '  "sah" - "Yakut"
                cultureTable.Add(new CultureTableEntry("sah", "sah"))
                '  "se" - "Sami (Northern)"
                cultureTable.Add(new CultureTableEntry("se", "sme"))
                '  "si" - "Sinhala"
                cultureTable.Add(new CultureTableEntry("si", "sin"))
                '  "sk" - "Slovak"
                cultureTable.Add(new CultureTableEntry("sk", "slk"))
                '  "sl" - "Slovenian"
                cultureTable.Add(new CultureTableEntry("sl", "slv"))
                '  "sma" - "Sami (Southern)"
                cultureTable.Add(new CultureTableEntry("sma", "smb"))
                '  "smj" - "Sami (Lule)"
                cultureTable.Add(new CultureTableEntry("smj", "smk"))
                '  "smn" - "Sami (Inari)"
                cultureTable.Add(new CultureTableEntry("smn", "smn"))
                '  "sms" - "Sami (Skolt)"
                cultureTable.Add(new CultureTableEntry("sms", "sms"))
                '  "sq" - "Albanian"
                cultureTable.Add(new CultureTableEntry("sq", "sqi"))
                '  "sr-Cyrl" - "Serbian (Cyrillic)"
                cultureTable.Add(new CultureTableEntry("sr", "srp"))
                '  "sr-Latn" - "Serbian (Latin)"
                cultureTable.Add(new CultureTableEntry("sr", "srp"))
                '  "sr" - "Serbian"
                cultureTable.Add(new CultureTableEntry("sr", "srp"))
                '  "sv" - "Swedish"
                cultureTable.Add(new CultureTableEntry("sv", "swe"))
                '  "sw" - "Kiswahili"
                cultureTable.Add(new CultureTableEntry("sw", "swa"))
                '  "syr" - "Syriac"
                cultureTable.Add(new CultureTableEntry("syr", "syr"))
                '  "ta" - "Tamil"
                cultureTable.Add(new CultureTableEntry("ta", "tam"))
                '  "te" - "Telugu"
                cultureTable.Add(new CultureTableEntry("te", "tel"))
                '  "tg-Cyrl" - "Tajik (Cyrillic)"
                cultureTable.Add(new CultureTableEntry("tg", "tgk"))
                '  "tg" - "Tajik"
                cultureTable.Add(new CultureTableEntry("tg", "tgk"))
                '  "th" - "Thai"
                cultureTable.Add(new CultureTableEntry("th", "tha"))
                '  "tk" - "Turkmen"
                cultureTable.Add(new CultureTableEntry("tk", "tuk"))
                '  "tn" - "Setswana"
                cultureTable.Add(new CultureTableEntry("tn", "tsn"))
                '  "tr" - "Turkish"
                cultureTable.Add(new CultureTableEntry("tr", "tur"))
                '  "tt" - "Tatar"
                cultureTable.Add(new CultureTableEntry("tt", "tat"))
                '  "tzm-Latn" - "Tamazight (Latin)"
                cultureTable.Add(new CultureTableEntry("tzm", "tzm"))
                '  "tzm" - "Tamazight"
                cultureTable.Add(new CultureTableEntry("tzm", "tzm"))
                '  "ug" - "Uyghur"
                cultureTable.Add(new CultureTableEntry("ug", "uig"))
                '  "uk" - "Ukrainian"
                cultureTable.Add(new CultureTableEntry("uk", "ukr"))
                '  "ur" - "Urdu"
                cultureTable.Add(new CultureTableEntry("ur", "urd"))
                '  "uz-Cyrl" - "Uzbek (Cyrillic)"
                cultureTable.Add(new CultureTableEntry("uz", "uzb"))
                '  "uz-Latn" - "Uzbek (Latin)"
                cultureTable.Add(new CultureTableEntry("uz", "uzb"))
                '  "uz" - "Uzbek"
                cultureTable.Add(new CultureTableEntry("uz", "uzb"))
                '  "vi" - "Vietnamese"
                cultureTable.Add(new CultureTableEntry("vi", "vie"))
                '  "wo" - "Wolof"
                cultureTable.Add(new CultureTableEntry("wo", "wol"))
                '  "xh" - "isiXhosa"
                cultureTable.Add(new CultureTableEntry("xh", "xho"))
                '  "yo" - "Yoruba"
                cultureTable.Add(new CultureTableEntry("yo", "yor"))
                '  "zh-CHS" - "Chinese (Simplified) Legacy"
                cultureTable.Add(new CultureTableEntry("zh", "zho"))
                '  "zh-CHT" - "Chinese (Traditional) Legacy"
                cultureTable.Add(new CultureTableEntry("zh", "zho"))
                '  "zh-Hans" - "Chinese (Simplified)"
                cultureTable.Add(new CultureTableEntry("zh", "zho"))
                '  "zh-Hant" - "Chinese (Traditional)"
                cultureTable.Add(new CultureTableEntry("zh", "zho"))
                '  "zh" - "Chinese"
                cultureTable.Add(new CultureTableEntry("zh", "zho"))
                '  "zu" - "isiZulu"
                cultureTable.Add(new CultureTableEntry("zu", "zul"))
            End If
        End Sub '   InitCultureTable
    End Class   '   LanguageAlias
' End Namespace   '   ExpressionMediaPlayer
' ..\Project_10\AdvancedMedia\1-AdvanceMedia\AdvMediaPlayer.Web\EncoderMedia\MediaPlayer\LanguageAlias.cs
