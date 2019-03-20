namespace TYS.Library.Image
{
    public enum Exif : int
    {
        ImageWidth = 0xA002,
        ImageHeight = 0xA003,
        ModelName = 0x010f, /* モデル名 */
        Orientation = 0x0112,
        ISOSpeedRate = 0x8827, /* ISOスピードレート */
        ExposureTime = 0x829a, /* 露出時間 */
        FNumber = 0x829d, /* Fナンバー */
        DateDataGeneration = 0x9003, /* データ生成日時 */
        ShutterSpeed = 0x9201, /* シャッタースピード */
        ApertureValue = 0x9202, /* 絞り値 */
        BrightnessValue = 0x9203, /* 輝度値 */
        SubjectDistance = 0x9206, /* 被写体距離 */
        Flash = 0x9209, /* フラッシュ */
        ExecutionImageWidth = 0xa002, /* 実行画像幅(横方向のピクセル数) */
        ExecutionImageHeight = 0xa003, /* 実行画像高さ（高さ方向のピクセル数） */
        ExposureMode = 0xa402, /* 露出モード */
        WhiteBalance = 0xa403, /* ホワイトバランス */
        ShootingSceneType = 0xa406  /* 撮影シーンタイプ */
    }
}
