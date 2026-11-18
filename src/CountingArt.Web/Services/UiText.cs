using System.Globalization;

namespace CountingArt.Web.Services;

public enum UiLanguage
{
    Chinese,
    English,
    French,
}

/// <summary>
/// UI text in Chinese, English and French. Switching language re-renders immediately (no page reload).
/// To add a language: add an enum value, a code in <see cref="Codes"/>, and one entry per key.
/// </summary>
public sealed class UiText
{
    public static readonly IReadOnlyDictionary<UiLanguage, string> Codes = new Dictionary<UiLanguage, string>
    {
        [UiLanguage.Chinese] = "zh",
        [UiLanguage.English] = "en",
        [UiLanguage.French] = "fr",
    };

    public static readonly IReadOnlyDictionary<UiLanguage, string> NativeNames = new Dictionary<UiLanguage, string>
    {
        [UiLanguage.Chinese] = "中文",
        [UiLanguage.English] = "English",
        [UiLanguage.French] = "Français",
    };

    private static readonly Dictionary<string, (string Zh, string En, string Fr)> Texts = new()
    {
        ["AppTitle"] = ("数一数：三角形", "Count the Triangles", "Compte les triangles"),
        ["Instructions"] = (
            "图中一共有多少个三角形？依次点击一个三角形的三个顶点，把它“抓”出来。先自己数，再检查。",
            "How many triangles are in the figure? Click the three corners of a triangle to catch it. Count on your own first, then check.",
            "Combien de triangles y a-t-il dans la figure ? Clique sur les trois sommets d'un triangle pour l'attraper. Compte d'abord tout seul, puis vérifie."),
        ["Shape"] = ("图形", "Shape", "Forme"),
        ["ShapeParallelogram"] = ("平行四边形", "Parallelogram", "Parallélogramme"),
        ["ShapeTriangle"] = ("大三角形", "Big triangle", "Grand triangle"),
        ["ShapeTrapezoid"] = ("梯形", "Trapezoid", "Trapèze"),
        ["Width"] = ("宽", "Width", "Largeur"),
        ["Rows"] = ("行数", "Rows", "Rangées"),
        ["Size"] = ("边长", "Side", "Côté"),
        ["Language"] = ("语言", "Language", "Langue"),
        ["FoundCount"] = ("你已经找到 {0} 个三角形。", "You have found {0} triangles.", "Tu as trouvé {0} triangles."),
        ["Up"] = ("尖朝上 ▲", "pointing up ▲", "pointe en haut ▲"),
        ["Down"] = ("尖朝下 ▼", "pointing down ▼", "pointe en bas ▼"),
        ["MsgNew"] = ("找到了！边长 {0}，{1}。", "Got one! Side {0}, {1}.", "Trouvé ! Côté {0}, {1}."),
        ["MsgAlready"] = ("这个已经找到过了。", "You already found that one.", "Tu l'as déjà trouvé."),
        ["MsgNotGrid"] = (
            "这三个点连不成网格上的三角形，再看看？",
            "These three points don't make a triangle on the grid. Look again?",
            "Ces trois points ne forment pas un triangle de la grille. Regarde encore ?"),
        ["MsgSidesNotDrawn"] = (
            "这个三角形有的边不在图上，不能算。",
            "Some sides of that triangle are not drawn in the figure, so it doesn't count.",
            "Certains côtés de ce triangle ne sont pas tracés, donc il ne compte pas."),
        ["ClearPoints"] = ("取消选点", "Clear points", "Effacer les points"),
        ["StartOver"] = ("重新开始", "Start over", "Recommencer"),
        ["CheckWork"] = ("检查一下", "Check my work", "Vérifier"),
        ["CheckTitle"] = (
            "按“边长 × 方向”分类，哪一类还有遗漏？",
            "Sorted by side and direction: which kinds still have some missing?",
            "Classés par côté et direction : quels types sont encore incomplets ?"),
        ["Kind"] = ("类别", "Kind", "Type"),
        ["YouFound"] = ("你找到", "You found", "Trouvés"),
        ["Status"] = ("情况", "Status", "État"),
        ["KindDone"] = ("✓ 全找到了", "✓ all found", "✓ tous trouvés"),
        ["KindMissing"] = ("还有遗漏", "some missing", "il en manque"),
        ["AllDone"] = ("太棒了！一个都没漏！", "Amazing, you found every one!", "Bravo, tu les as tous trouvés !"),
        ["ShowAnswers"] = ("看答案", "Show answers", "Voir les réponses"),
        ["HideAnswers"] = ("收起答案", "Hide answers", "Masquer les réponses"),
        ["Total"] = ("一共", "Total", "Total"),
        ["TotalCount"] = ("图中一共有 {0} 个三角形。", "There are {0} triangles in the figure.", "Il y a {0} triangles dans la figure."),
        ["Step"] = ("第 {0} / {1} 个", "{0} of {1}", "{0} sur {1}"),
        ["Previous"] = ("上一个", "Previous", "Précédent"),
        ["Next"] = ("下一个", "Next", "Suivant"),
        ["YouFoundThis"] = ("这个你找到了 ✓", "You found this one ✓", "Tu as trouvé celui-ci ✓"),
        ["YouMissedThis"] = ("这个漏掉了", "You missed this one", "Tu as manqué celui-ci"),
        ["PatternTitle"] = (
            "找规律：图形变大时，三角形总数怎么变？",
            "Find the pattern: how does the total change as the figure grows?",
            "Trouve la régularité : comment le total change-t-il quand la figure grandit ?"),
        ["Increase"] = ("比上一个多", "Increase", "Augmentation"),
    };

    public UiLanguage Language { get; private set; } = UiLanguage.English;

    public event Action? Changed;

    public string this[string key] => Get(key);

    public string Format(string key, params object[] args) =>
        string.Format(CultureInfo.InvariantCulture, Get(key), args);

    public void SetLanguage(UiLanguage language)
    {
        if (Language != language)
        {
            Language = language;
            Changed?.Invoke();
        }
    }

    public static UiLanguage FromCode(string? code)
    {
        var prefix = (code ?? string.Empty).Split('-', '_')[0].ToLowerInvariant();
        return Codes.FirstOrDefault(kv => kv.Value == prefix, new(UiLanguage.English, "en")).Key;
    }

    private string Get(string key)
    {
        if (!Texts.TryGetValue(key, out var t))
        {
            return key;
        }

        return Language switch
        {
            UiLanguage.Chinese => t.Zh,
            UiLanguage.French => t.Fr,
            _ => t.En,
        };
    }
}
