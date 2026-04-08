# Unity UI 设置说明

## 1. RectTransform 的 Anchor（锚点）设置

### 什么是 Anchor？
Anchor（锚点）决定了 UI 元素如何相对于父容器定位和缩放。

### 如何设置"左下角到右上角（拉伸填满）"：

1. **选中你的 RawImage（BackgroundVideoImage）**
2. **在 Inspector 中找到 RectTransform 组件**
3. **点击左上角的 Anchor Presets 按钮**（看起来像一个小方块图标）
4. **按住 Shift + Alt，然后点击右下角的选项**（或者直接点击"stretch-stretch"选项）

   这样会同时设置：
   - Anchor Min: (0, 0) - 左下角
   - Anchor Max: (1, 1) - 右上角
   - 这样元素会填满整个父容器

5. **或者手动设置**：
   - Anchor Min: X=0, Y=0
   - Anchor Max: X=1, Y=1
   - Left, Right, Top, Bottom 都设为 0

### 效果：
- UI 元素会填满整个 Canvas
- 无论屏幕大小如何变化，都会自动适应

---

## 2. Hierarchy（层级）顺序

### 什么是 Hierarchy？
Hierarchy 窗口显示场景中所有对象的层级关系。**在 Unity UI 中，越靠下的元素会显示在越上层**。

### 如何确保背景在最底部：

1. **打开 Hierarchy 窗口**（通常在左侧）
2. **找到你的 Canvas**
3. **展开 Canvas，查看所有子对象**
4. **找到 BackgroundVideoImage（背景视频）**
5. **将它拖到 Canvas 子对象列表的最上面**（第一个位置）

   或者：
   - 右键点击 BackgroundVideoImage
   - 选择 "Move To" → "Top"

### 为什么重要？
- Unity 按照 Hierarchy 的顺序从下往上渲染
- 背景应该在最下面，这样其他 UI（按钮、文字等）会显示在背景上面
- 如果背景在上面，会遮挡其他 UI 元素

### 正确的顺序应该是：
```
Canvas
├── BackgroundVideoImage (背景 - 最下面)
├── SettingsPanel (设置面板)
├── VolumeSlider (音量滑块)
├── BackButton (返回按钮)
└── ... (其他 UI 元素)
```

---

## 快速检查清单

✅ BackgroundVideoImage 的 Anchor 设置为 (0,0) 到 (1,1)
✅ BackgroundVideoImage 在 Hierarchy 中位于 Canvas 子对象的最上面
✅ 其他 UI 元素在 BackgroundVideoImage 下面
✅ RawImage 的 Texture 已设置为 RenderTexture

