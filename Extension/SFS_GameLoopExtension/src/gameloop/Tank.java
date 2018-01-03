package gameloop;

public class Tank extends GameObject {
	private double _rotation;
	private boolean _alive;
	private float _deathDuration;

	public Tank(double x, double y, int w, int h) {
		super(x, y, w, h);
		super.SetType("tanktype");
		_alive = true;
		_deathDuration = 0;
	}

	public double GetR() {
		return _rotation;
	}

	public void SetR(double value) {
		_rotation = value;
	}

	public void SetProperties(double x, double y, double rotation) {
		this._rotation = rotation;
		super.SetProperties(x, y);
	}

	public boolean IsAlive() {
		return _alive;
	}

	public void ReGeneration() {
		_alive = true;
	}

	public void Death() {
		_deathDuration = RoomExtension.DEATH_DURATION;
		_alive = false;
	}

	public void UpdateDeathDuration(float deltaTime) {
		if (!_alive)
			_deathDuration -= deltaTime;
	}

	public float GetDeathRemainDuration() {
		return _deathDuration;
	}
}
